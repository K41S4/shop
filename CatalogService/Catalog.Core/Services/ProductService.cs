using AutoMapper;
using Catalog.Core.Entities;
using Catalog.Core.Exceptions;
using Catalog.Core.Messaging;
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
        private readonly IProductUpdatePublisher updatePublisher;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="productRepo">The product repository.</param>
        /// <param name="categoryRepo">The category repository.</param>
        /// <param name="updatePublisher">Product update publisher.</param>
        /// <param name="mapper">Mapper.</param>
        public ProductService(IProductRepository productRepo, ICategoryRepository categoryRepo, IProductUpdatePublisher updatePublisher, IMapper mapper)
        {
            this.productRepo = productRepo;
            this.categoryRepo = categoryRepo;
            this.updatePublisher = updatePublisher;
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<Product?> GetProduct(int id)
        {
            return await this.productRepo.GetProduct(id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Product>> GetProducts(int categoryId, int page, int limit)
        {
            var category = await this.categoryRepo.GetCategory(categoryId);
            if (category is null)
            {
                throw new InvalidCategoryException();
            }

            return await this.productRepo.GetProducts(categoryId, page, limit);
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
        public async Task RemoveProduct(int id)
        {
            var product = await this.productRepo.GetProduct(id);
            if (product is null)
            {
                throw new NotFoundException($"Product with {id} id was not found.");
            }

            await this.productRepo.RemoveProduct(id);
        }

        /// <inheritdoc/>
        public async Task UpdateProduct(Product product)
        {
            var productFromDB = await this.productRepo.GetProduct(product.Id);
            if (productFromDB is null)
            {
                throw new NotFoundException($"Product with {product.Id} id was not found.");
            }

            var category = await this.categoryRepo.GetCategory(product.CategoryId);

            if (category is null)
            {
                throw new InvalidCategoryException();
            }

            await this.updatePublisher.PublishProductUpdateAsync(this.mapper.Map<ProductUpdatedMessage>(product));
        }
    }
}
