using Catalog.Core.Entities;

namespace Catalog.Core.Services.Interfaces
{
    public interface ICategoryService
    {
        public Task<Category?> GetCategory(int id);

        public Task<IEnumerable<Category>> GetCategories();

        public Task AddCategory(Category category);

        public Task RemoveCategory(int id);

        public Task UpdateCategory(Category category);
    }
}
