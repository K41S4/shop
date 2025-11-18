using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace CartApp.Messaging;

/// <summary>
/// Kafka consumer for product update messages with retry and dead letter queue support.
/// </summary>
public class ProductUpdateConsumer : BackgroundService
{
    private readonly IConsumer<string, string> consumer;
    private readonly IServiceProvider serviceProvider;
    private readonly ILogger<ProductUpdateConsumer> logger;
    private readonly KafkaConfig kafkaConfig;
    private readonly string topicName;
    private readonly string deadLetterTopic;
    private readonly IProducer<string, string> dlqProducer;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductUpdateConsumer"/> class.
    /// </summary>
    /// <param name="kafkaConfig">Kafka configuration.</param>
    /// <param name="serviceProvider">Service provider.</param>
    /// <param name="logger">Logger.</param>
    public ProductUpdateConsumer(
        IOptions<KafkaConfig> kafkaConfig,
        IServiceProvider serviceProvider,
        ILogger<ProductUpdateConsumer> logger)
    {
        this.serviceProvider = serviceProvider;
        this.logger = logger;
        this.kafkaConfig = kafkaConfig.Value;
        this.topicName = this.kafkaConfig.ProductUpdateTopic;
        this.deadLetterTopic = this.kafkaConfig.DeadLetterTopic;

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = this.kafkaConfig.BootstrapServers,
            GroupId = this.kafkaConfig.ConsumerGroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false,
            EnablePartitionEof = true,
        };

        this.consumer = new ConsumerBuilder<string, string>(consumerConfig)
            .SetErrorHandler((_, error) =>
            {
                this.logger.LogError("Kafka consumer error: {Error}", error);
            })
            .Build();

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = this.kafkaConfig.BootstrapServers,
            Acks = Acks.All,
        };

        this.dlqProducer = new ProducerBuilder<string, string>(producerConfig).Build();
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await Task.Yield();
            this.consumer.Subscribe(this.topicName);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = this.consumer.Consume(stoppingToken);

                    if (result.IsPartitionEOF)
                    {
                        continue;
                    }

                    await this.ProcessMessageAsync(result, stoppingToken);
                }
                catch (Exception exception)
                {
                    if (exception is OperationCanceledException)
                    {
                        break;
                    }

                    this.logger.LogError(exception, "Exception while consuming from Kafka");
                }
            }
        }
        finally
        {
            this.consumer.Close();
            this.dlqProducer.Flush(TimeSpan.FromSeconds(10));
            this.dlqProducer.Dispose();
        }
    }

    private async Task ProcessMessageAsync(ConsumeResult<string, string> result, CancellationToken cancellationToken)
    {
        var retryCount = 0;
        var success = false;

        while (retryCount <= this.kafkaConfig.MaxRetries && !success)
        {
            try
            {
                var message = JsonSerializer.Deserialize<ProductUpdatedMessage>(result.Message.Value);

                if (message is null)
                {
                    await this.SendToDeadLetterQueueAsync(result, "Deserialization failed", cancellationToken);
                    this.consumer.Commit(result);
                    return;
                }

                using var scope = this.serviceProvider.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<IProductUpdateHandler>();

                await handler.HandleProductUpdateAsync(message, cancellationToken);
                this.consumer.Commit(result);

                success = true;

                this.logger.LogInformation(
                    "Processed product update. ProductId: {ProductId}, Offset: {Offset}",
                    message.ProductId,
                    result.Offset);
            }
            catch (Exception ex)
            {
                retryCount++;

                if (retryCount > this.kafkaConfig.MaxRetries)
                {
                    await this.SendToDeadLetterQueueAsync(result, ex.Message, cancellationToken);
                    this.consumer.Commit(result);
                }
                else
                {
                    var delay = this.kafkaConfig.RetryDelayMs * (int)Math.Pow(2, retryCount - 1);
                    await Task.Delay(delay, cancellationToken);
                }
            }
        }
    }

    private async Task SendToDeadLetterQueueAsync(
        ConsumeResult<string, string> originalResult,
        string errorReason,
        CancellationToken cancellationToken)
    {
        try
        {
            var dlqMessage = new Message<string, string>
            {
                Key = originalResult.Message.Key,
                Value = originalResult.Message.Value,
                Headers = new Headers
                {
                    { "reason", System.Text.Encoding.UTF8.GetBytes(errorReason) },
                },
            };

            await this.dlqProducer.ProduceAsync(this.deadLetterTopic, dlqMessage, cancellationToken);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to send message to DLQ.");
        }
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        this.consumer.Dispose();
        this.dlqProducer.Dispose();
        base.Dispose();
    }
}