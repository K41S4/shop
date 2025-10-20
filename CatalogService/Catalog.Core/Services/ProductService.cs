using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Services.Interfaces;

namespace Catalog.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository repository;

        public ProductService(IProductRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Product?> GetProduct(int id)
        {
            return await this.repository.GetProduct(id);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await this.repository.GetProducts();
        }

        public async Task AddProduct(Product product)
        {
            await this.repository.AddProduct(product);
        }

        public async Task RemoveProduct(int id)
        {
            await this.repository.RemoveProduct(id);
        }

        public async Task UpdateProduct(Product product)
        {
            await this.repository.UpdateProduct(product);
        }
    }
}
