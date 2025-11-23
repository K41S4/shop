namespace Catalog.Core.Messaging;

/// <summary>
/// Interface for publishing product update messages.
/// </summary>
public interface IProductUpdatePublisher
{
    /// <summary>
    /// Publishes a product update message.
    /// </summary>
    /// <param name="message">The product update message.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task representing the async operation.</returns>
    Task PublishProductUpdateAsync(ProductUpdatedMessage message, CancellationToken cancellationToken = default);
}