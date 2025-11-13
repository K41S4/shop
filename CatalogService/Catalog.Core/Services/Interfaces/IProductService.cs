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
        /// Updates an existing product.
        /// </summary>
        /// <param name="product">The product to update.</param>
        /// <returns>Task.</returns>
        public Task UpdateProduct(Product product);
    }
}
