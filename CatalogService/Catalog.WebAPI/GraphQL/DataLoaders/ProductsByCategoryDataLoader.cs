using Catalog.Core.Entities;
using Catalog.Core.Services.Interfaces;

namespace Catalog.WebAPI.GraphQL.DataLoaders;

/// <summary>
/// DataLoader for loading products by category ID.
/// </summary>
public class ProductsByCategoryDataLoader : BatchDataLoader<int, IEnumerable<Product>>
{
    private readonly IProductService productService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductsByCategoryDataLoader"/> class.
    /// </summary>
    /// <param name="batchScheduler">The batch scheduler.</param>
    /// <param name="productService">The product service.</param>
    /// <param name="options">The data loader options.</param>
    public ProductsByCategoryDataLoader(
        IBatchScheduler batchScheduler,
        IProductService productService,
        DataLoaderOptions options)
        : base(batchScheduler, options)
    {
        this.productService = productService;
    }

    /// <inheritdoc/>
    protected override async Task<IReadOnlyDictionary<int, IEnumerable<Product>>> LoadBatchAsync(
        IReadOnlyList<int> keys,
        CancellationToken cancellationToken)
    {
        try
        {
            var products = await this.productService.GetProductsByCategoryIds(keys);
            var groupedProducts = products
                .GroupBy(p => p.CategoryId)
                .ToDictionary(g => g.Key, g => g.AsEnumerable());

            return keys.ToDictionary(
                categoryId => categoryId,
                categoryId => groupedProducts.GetValueOrDefault(categoryId) ?? Enumerable.Empty<Product>());
        }
        catch
        {
            return keys.ToDictionary(id => id, _ => Enumerable.Empty<Product>());
        }
    }
}