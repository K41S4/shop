using Catalog.Core.Entities;

namespace Catalog.Core.Services.Interfaces
{
    public interface IProductService
    {
        public Task<Product?> GetProduct(int id);
        public Task<IEnumerable<Product>> GetProducts();
        public Task AddProduct(Product product);
        public Task RemoveProduct(int id);
        public Task UpdateProduct(Product product);
    }
}
