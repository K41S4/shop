using Catalog.Core.Entities;
using Catalog.Core.Services.Interfaces;

namespace Catalog.WebAPI.GraphQL.DataLoaders;

/// <summary>
/// DataLoader for categories.
/// </summary>
public class CategoryDataLoader : BatchDataLoader<int, Category?>
{
    private readonly ICategoryService categoryService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryDataLoader"/> class.
    /// </summary>
    /// <param name="batchScheduler">The batch scheduler.</param>
    /// <param name="categoryService">The category service.</param>
    /// <param name="options">The data loader options.</param>
    public CategoryDataLoader(
        IBatchScheduler batchScheduler,
        ICategoryService categoryService,
        DataLoaderOptions options)
        : base(batchScheduler, options)
    {
        this.categoryService = categoryService;
    }

    /// <inheritdoc/>
    protected override async Task<IReadOnlyDictionary<int, Category?>> LoadBatchAsync(
        IReadOnlyList<int> keys,
        CancellationToken cancellationToken)
    {
        var categories = await this.categoryService.GetCategoriesByIds(keys);
        var categoriesDict = categories.ToDictionary(c => c.Id, c => c);

        return keys.ToDictionary(
            id => id,
            id => categoriesDict.GetValueOrDefault(id));
    }
}