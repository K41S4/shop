using Catalog.Core.Entities;

namespace Catalog.Core.Repositories
{
    public interface IProductRepository
    {
        public Task<Product?> GetProduct(int id);
        public Task<IEnumerable<Product>> GetProducts();
        public Task AddProduct(Product product);
        public Task RemoveProduct(int productId);
        public Task UpdateProduct(Product product);
    }
}
