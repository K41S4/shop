using AutoMapper;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Persistence.DBContext;
using Catalog.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation for product data access.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogDBContext dbContext;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        public ProductRepository(CatalogDBContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<Product?> GetProduct(int id)
        {
            var productEntity = await this.dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            return this.mapper.Map<Product>(productEntity);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Product>> GetProducts()
        {
            var productEntities = await this.dbContext.Products.ToListAsync();
            return this.mapper.Map<IEnumerable<Product>>(productEntities);
        }

        /// <inheritdoc/>
        public async Task AddProduct(Product product)
        {
            var productEntity = this.mapper.Map<ProductEntity>(product);
            await this.dbContext.Products.AddAsync(productEntity);
            await this.dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task RemoveProduct(int productId)
        {
            var productEntity = await this.dbContext.Products.FindAsync(productId);
            if (productEntity is null)
            {
                return;
            }

            this.dbContext.Products.Remove(productEntity);
            await this.dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateProduct(Product product)
        {
            var productEntity = await this.dbContext.Products.FindAsync(product.Id);
            if (productEntity is null)
            {
                return;
            }

            productEntity.Price = product.Price.Value;
            productEntity.Name = product.Name.Value!;
            productEntity.Amount = product.Amount.Value;
            productEntity.Description = product.Description;
            productEntity.Image = product.Image?.Value;
            productEntity.CategoryId = product.CategoryId;

            await this.dbContext.SaveChangesAsync();
        }
    }
}
