using AutoMapper;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Persistence.DBContext;
using Catalog.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogDBContext dbContext;
        private readonly IMapper mapper;

        public ProductRepository(CatalogDBContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<Product?> GetProduct(int id)
        {
            var productEntity = await this.dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            return this.mapper.Map<Product>(productEntity);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var productEntities = await this.dbContext.Products.ToListAsync();
            return this.mapper.Map<IEnumerable<Product>>(productEntities);

        }

        public async Task AddProduct(Product product)
        {
            var productEntity = this.mapper.Map<ProductEntity>(product);
            await this.dbContext.Products.AddAsync(productEntity);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task RemoveProduct(int productId)
        {
            var productEntity = await this.dbContext.Products.FindAsync(productId);
            if (productEntity is null) return;

            dbContext.Products.Remove(productEntity);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateProduct(Product product)
        {
            var productEntity = await this.dbContext.Products.FindAsync(product.Id);
            if (productEntity is null) return;

            productEntity.Price = product.Price.Value;
            productEntity.Name = product.Name.Value;
            productEntity.Amount = product.Amount.Value;
            productEntity.Description = product.Description;
            productEntity.Image = product.Image;
            productEntity.CategoryId = product.CategoryId;

            await dbContext.SaveChangesAsync();
        }
    }
}
