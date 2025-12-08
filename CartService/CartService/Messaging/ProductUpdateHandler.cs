using CartApp.BusinessLogic.Services;

namespace CartApp.Messaging;

/// <summary>
/// Handler for product update messages from Kafka.
/// </summary>
public class ProductUpdateHandler : IProductUpdateHandler
{
    private readonly ICartService cartService;
    private readonly ILogger<ProductUpdateHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductUpdateHandler"/> class.
    /// </summary>
    /// <param name="cartService">The cart service.</param>
    /// <param name="logger">Logger.</param>
    public ProductUpdateHandler(ICartService cartService, ILogger<ProductUpdateHandler> logger)
    {
        this.cartService = cartService;
        this.logger = logger;
    }

    /// <inheritdoc/>
    public async Task HandleProductUpdateAsync(ProductUpdatedMessage message, CancellationToken cancellationToken = default)
    {
        await this.cartService.UpdateCartItem(message.ProductId, message.Name, message.Price, message.ImageUrl);

        this.logger.LogInformation("Successfully updated cart items for ProductId: {ProductId}", message.ProductId);
    }
}