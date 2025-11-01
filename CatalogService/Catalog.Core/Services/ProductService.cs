using Catalog.Core.Entities;
using Catalog.Core.Exceptions;
using Catalog.Core.Repositories;
using Catalog.Core.Services.Interfaces;

namespace Catalog.Core.Services
{
    /// <summary>
    /// Service for managing product operations.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepo;
        private readonly ICategoryRepository categoryRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="productRepo">The product repository.</param>
        /// <param name="categoryRepo">The category repository.</param>
        public ProductService(IProductRepository productRepo, ICategoryRepository categoryRepo)
        {
            this.productRepo = productRepo;
            this.categoryRepo = categoryRepo;
        }

        /// <inheritdoc/>
        public async Task<Product?> GetProduct(int id)
        {
            return await this.productRepo.GetProduct(id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await this.productRepo.GetProducts();
        }

        /// <inheritdoc/>
        public async Task AddProduct(Product product)
        {
            var category = await this.categoryRepo.GetCategory(product.CategoryId);

            if (category is null)
            {
                throw new InvalidCategoryException();
            }

            await this.productRepo.AddProduct(product);
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveProduct(int id)
        {
            return await this.productRepo.RemoveProduct(id);
        }

        /// <inheritdoc/>
        public async Task UpdateProduct(Product product)
        {
            var category = await this.categoryRepo.GetCategory(product.CategoryId);

            if (category is null)
            {
                throw new InvalidCategoryException();
            }

            await this.productRepo.UpdateProduct(product);
        }
    }
}
