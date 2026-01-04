using Catalog.Core.Entities;
using Catalog.Core.Services.Interfaces;
using HotChocolate.Authorization;

namespace Catalog.WebAPI.GraphQL.Queries;

/// <summary>
/// GraphQL queries for catalog operations.
/// </summary>
public class CatalogQuery
{
    /// <summary>
    /// Gets all categories.
    /// </summary>
    /// <param name="categoryService">The category service.</param>
    /// <returns>A collection of categories.</returns>
    [Authorize(Policy = "Read")]
    [GraphQLDescription("Gets a list of all categories.")]
    public async Task<IEnumerable<Category>> Categories([Service] ICategoryService categoryService)
    {
        return await categoryService.GetCategories();
    }

    /// <summary>
    /// Gets products with filtering by category id and pagination.
    /// </summary>
    /// <param name="productService">The product service.</param>
    /// <param name="categoryId">Category id to filter by.</param>
    /// <param name="page">The page number (0-based).</param>
    /// <param name="limit">The number of items per page.</param>
    /// <returns>A collection of products.</returns>
    [Authorize(Policy = "Read")]
    [GraphQLDescription("Gets a list of products filtered by category id with pagination. Products include their categories as subgraphs.")]
    public async Task<IEnumerable<Product>> Products(
        [Service] IProductService productService,
        int categoryId,
        int page,
        int limit)
    {
        return await productService.GetProducts(categoryId, page, limit);
    }
}