namespace CartApp.Messaging;

/// <summary>
/// Interface for handling product update messages.
/// </summary>
public interface IProductUpdateHandler
{
    /// <summary>
    /// Handles a product update message.
    /// </summary>
    /// <param name="message">The product update message.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task.</returns>
    Task HandleProductUpdateAsync(ProductUpdatedMessage message, CancellationToken cancellationToken = default);
}