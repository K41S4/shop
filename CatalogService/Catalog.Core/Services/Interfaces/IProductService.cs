using Catalog.Core.Entities;

namespace Catalog.Core.Services.Interfaces
{
    /// <summary>
    /// Defines the interface for product service.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Gets a product by its id.
        /// </summary>
        /// <param name="id">The product id.</param>
        /// <returns>The product if found, otherwise null.</returns>
        public Task<Product?> GetProduct(int id);

        /// <summary>
        /// Gets multiple products by their ids.
        /// </summary>
        /// <param name="ids">The product ids.</param>
        /// <returns>A collection of found products.</returns>
        public Task<IEnumerable<Product>> GetProductsByIds(IReadOnlyList<int> ids);

        /// <summary>
        /// Gets products with filtering.
        /// </summary>
        /// <param name="categoryId">The category id to filter by.</param>
        /// <param name="page">The requested page.</param>
        /// <param name="limit">The amount of products per page.</param>
        /// <returns>A collection of products.</returns>
        public Task<IEnumerable<Product>> GetProducts(int categoryId, int page, int limit);

        /// <summary>
        /// Adds a new product.
        /// </summary>
        /// <param name="product">The product to add.</param>
        /// <returns>Task.</returns>
        public Task AddProduct(Product product);

        /// <summary>
        /// Removes a product by its id.
        /// </summary>
        /// <param name="id">The product id.</param>
        /// <returns>Task.</returns>
        public Task RemoveProduct(int id);

        /// <summary>
        /// Gets products for multiple categories.
        /// </summary>
        /// <param name="categoryIds">The category ids.</param>
        /// <returns>A collection of products for the specified categories.</returns>
        public Task<IEnumerable<Product>> GetProductsByCategoryIds(IReadOnlyList<int> categoryIds);

        /// <summary>
        /// Removes all products by category id.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <returns>Task.</returns>
        public Task RemoveProductsByCategoryId(int categoryId);

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="product">The product to update.</param>
        /// <returns>Task.</returns>
        public Task UpdateProduct(Product product);
    }
}
