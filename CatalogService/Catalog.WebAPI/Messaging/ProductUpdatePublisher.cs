using Catalog.Core.Messaging;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Catalog.WebAPI.Messaging;

/// <summary>
/// Kafka publisher for product update messages.
/// </summary>
public class ProductUpdatePublisher : IProductUpdatePublisher, IDisposable
{
    private readonly IProducer<string, string> producer;
    private readonly ILogger<ProductUpdatePublisher> logger;
    private readonly string topicName;
    private readonly int flushTimeoutSeconds;
    private bool disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductUpdatePublisher"/> class.
    /// </summary>
    /// <param name="kafkaConfig">Kafka configuration.</param>
    /// <param name="logger">Logger.</param>
    public ProductUpdatePublisher(IOptions<KafkaConfig> kafkaConfig, ILogger<ProductUpdatePublisher> logger)
    {
        this.logger = logger;
        var config = kafkaConfig.Value;
        this.topicName = config.ProductUpdateTopic;
        this.flushTimeoutSeconds = config.ProducerFlushTimeoutSeconds;

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = config.BootstrapServers,
            Acks = Acks.All,
            EnableIdempotence = true,
            MessageSendMaxRetries = config.MessageSendMaxRetries,
            RetryBackoffMs = config.RetryBackoffMs,
        };

        this.producer = new ProducerBuilder<string, string>(producerConfig)
            .SetErrorHandler((_, error) =>
            {
                this.logger.LogError("Kafka producer error: {Error}", error);
            })
            .Build();
    }

    /// <inheritdoc/>
    public async Task PublishProductUpdateAsync(ProductUpdatedMessage message, CancellationToken cancellationToken = default)
    {
        try
        {
            var messageJson = JsonSerializer.Serialize(message);
            var kafkaMessage = new Message<string, string>
            {
                Key = message.ProductId.ToString(),
                Value = messageJson,
            };

            var deliveryResult = await this.producer.ProduceAsync(this.topicName, kafkaMessage, cancellationToken);

            this.logger.LogInformation(
                "Product update message published. ProductId: {ProductId}, Offset: {Offset}",
                message.ProductId,
                deliveryResult.Offset);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error publishing product update message. ProductId: {ProductId}", message.ProductId);
            throw;
        }
    }

    /// <summary>
    /// Disposes the test resources.
    /// </summary>
    /// <param name="disposing">Whether disposing is in progress.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                this.producer.Flush(TimeSpan.FromSeconds(this.flushTimeoutSeconds));
                this.producer.Dispose();
            }

            this.disposed = true;
        }
    }

    /// <summary>
    /// Disposes the test resources.
    /// </summary>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }
}