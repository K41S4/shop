namespace Catalog.WebAPI.Messaging;

/// <summary>
/// Configuration for Kafka connection.
/// </summary>
public class KafkaConfig
{
    /// <summary>
    /// Gets or sets the Kafka bootstrap servers.
    /// </summary>
    public required string BootstrapServers { get; set; }

    /// <summary>
    /// Gets or sets the topic name for product updates.
    /// </summary>
    public required string ProductUpdateTopic { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of retries for message sending.
    /// </summary>
    public int MessageSendMaxRetries { get; set; } = 3;

    /// <summary>
    /// Gets or sets the retry backoff delay in milliseconds.
    /// </summary>
    public int RetryBackoffMs { get; set; } = 100;

    /// <summary>
    /// Gets or sets the producer flush timeout in seconds.
    /// </summary>
    public int ProducerFlushTimeoutSeconds { get; set; } = 10;
}