using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Services.Interfaces;

namespace Catalog.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository repository;

        public CategoryService(ICategoryRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Category?> GetCategory(int id)
        {
            return await this.repository.GetCategory(id);
        }

        public async Task <IEnumerable<Category>> GetCategories()
        {
            return await this.repository.GetCategories();
        }

        public async Task AddCategory(Category category)
        {
            await this.repository.AddCategory(category);
        }

        public async Task RemoveCategory(int id)
        {
            await this.repository.RemoveCategory(id);
        }

        public async Task UpdateCategory(Category category)
        {
            await this.repository.UpdateCategory(category);
        }
    }
}
