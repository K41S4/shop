using Catalog.Core.Entities;

namespace Catalog.Core.Repositories
{
    public interface ICategoryRepository
    {
        public Task<Category?> GetCategory(int id);
        public Task<IEnumerable<Category>> GetCategories();
        public Task AddCategory(Category category);
        public Task RemoveCategory(int categoryId);
        public Task UpdateCategory(Category category);
    }
}
