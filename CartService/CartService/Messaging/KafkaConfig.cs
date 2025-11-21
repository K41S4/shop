namespace CartApp.Messaging;

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
    /// Gets or sets the consumer group ID.
    /// </summary>
    public required string ConsumerGroupId { get; set; }

    /// <summary>
    /// Gets or sets the dead letter topic name.
    /// </summary>
    public required string DeadLetterTopic { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of retries for message processing.
    /// </summary>
    public int MaxRetries { get; set; } = 3;

    /// <summary>
    /// Gets or sets the initial retry delay in milliseconds.
    /// </summary>
    public int RetryDelayMs { get; set; } = 1000;

    /// <summary>
    /// Gets or sets the maximum number of retries for DLQ.
    /// </summary>
    public int MaxDlqRetries { get; set; } = 3;

    /// <summary>
    /// Gets or sets the initial DLQ retry delay in milliseconds.
    /// </summary>
    public int DlqRetryDelayMs { get; set; } = 1000;

    /// <summary>
    /// Gets or sets the producer flush timeout in seconds.
    /// </summary>
    public int ProducerFlushTimeoutSeconds { get; set; } = 10;
}