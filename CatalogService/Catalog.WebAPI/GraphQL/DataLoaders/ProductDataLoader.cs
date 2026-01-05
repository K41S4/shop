using Catalog.Core.Entities;
using Catalog.Core.Services.Interfaces;

namespace Catalog.WebAPI.GraphQL.DataLoaders;

/// <summary>
/// DataLoader for products.
/// </summary>
public class ProductDataLoader : BatchDataLoader<int, Product?>
{
    private readonly IProductService productService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductDataLoader"/> class.
    /// </summary>
    /// <param name="batchScheduler">The batch scheduler.</param>
    /// <param name="productService">The product service.</param>
    /// <param name="options">The data loader options.</param>
    public ProductDataLoader(
        IBatchScheduler batchScheduler,
        IProductService productService,
        DataLoaderOptions options)
        : base(batchScheduler, options)
    {
        this.productService = productService;
    }

    /// <inheritdoc/>
    protected override async Task<IReadOnlyDictionary<int, Product?>> LoadBatchAsync(
        IReadOnlyList<int> keys,
        CancellationToken cancellationToken)
    {
        var products = await this.productService.GetProductsByIds(keys);
        var productsDict = products.ToDictionary(p => p.Id, p => p);

        return keys.ToDictionary(
            id => id,
            id => productsDict.GetValueOrDefault(id));
    }
}