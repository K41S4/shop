using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;

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
    private readonly ResiliencePipeline processMessagePipeline;
    private readonly ResiliencePipeline dlqPipeline;
    private bool disposed;

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

        this.processMessagePipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder().Handle<Exception>(ex => ex is not OperationCanceledException),
                MaxRetryAttempts = this.kafkaConfig.MaxRetries,
                Delay = TimeSpan.FromMilliseconds(this.kafkaConfig.RetryDelayMs),
                BackoffType = DelayBackoffType.Exponential,
                OnRetry = args =>
                {
                    this.logger.LogWarning(
                        args.Outcome.Exception,
                        "Retrying to process message. Attempt {AttemptNumber}",
                        args.AttemptNumber);
                    return ValueTask.CompletedTask;
                },
            })
            .Build();

        this.dlqPipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder().Handle<Exception>(ex => ex is not OperationCanceledException),
                MaxRetryAttempts = this.kafkaConfig.MaxDlqRetries,
                Delay = TimeSpan.FromMilliseconds(this.kafkaConfig.DlqRetryDelayMs),
                BackoffType = DelayBackoffType.Exponential,
                OnRetry = args =>
                {
                    this.logger.LogWarning(
                        args.Outcome.Exception,
                        "Retrying to send to DLQ. Attempt {AttemptNumber}",
                        args.AttemptNumber);
                    return ValueTask.CompletedTask;
                },
            })
            .Build();
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        if (this.disposed)
        {
            return;
        }

        this.consumer.Dispose();
        this.dlqProducer.Dispose();
        this.disposed = true;
        base.Dispose();
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
                        await Task.Delay(TimeSpan.FromMilliseconds(100), stoppingToken);
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
            this.dlqProducer.Flush(TimeSpan.FromSeconds(this.kafkaConfig.ProducerFlushTimeoutSeconds));
        }
    }

    private async Task ProcessMessageAsync(ConsumeResult<string, string> result, CancellationToken cancellationToken)
    {
        try
        {
            await this.processMessagePipeline.ExecuteAsync(
                async token =>
            {
                var message = JsonSerializer.Deserialize<ProductUpdatedMessage>(result.Message.Value);

                if (message is null)
                {
                    await this.SendToDeadLetterQueueAsync(result, "Deserialization failed", token);
                    this.consumer.Commit(result);
                    return;
                }

                using var scope = this.serviceProvider.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<IProductUpdateHandler>();

                await handler.HandleProductUpdateAsync(message, token);
                this.consumer.Commit(result);

                this.logger.LogInformation(
                    "Processed product update. ProductId: {ProductId}, Offset: {Offset}",
                    message.ProductId,
                    result.Offset);
                }, cancellationToken);
        }
        catch (Exception ex)
        {
            await this.SendToDeadLetterQueueAsync(result, ex.Message, cancellationToken);
            this.consumer.Commit(result);
        }
    }

    private async Task SendToDeadLetterQueueAsync(
        ConsumeResult<string, string> originalResult,
        string errorReason,
        CancellationToken cancellationToken)
    {
        await this.dlqPipeline.ExecuteAsync(
            async token =>
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

            await this.dlqProducer.ProduceAsync(this.deadLetterTopic, dlqMessage, token);
            }, cancellationToken);
    }
}
