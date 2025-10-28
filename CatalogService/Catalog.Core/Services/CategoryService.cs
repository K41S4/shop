using Catalog.Core.Entities;
using Catalog.Core.Exceptions;
using Catalog.Core.Repositories;
using Catalog.Core.Services.Interfaces;

namespace Catalog.Core.Services
{
    /// <summary>
    /// Service for managing category operations.
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryService"/> class.
        /// </summary>
        /// <param name="repository">The category repository.</param>
        public CategoryService(ICategoryRepository repository)
        {
            this.repository = repository;
        }

        /// <inheritdoc/>
        public async Task<Category?> GetCategory(int id)
        {
            return await this.repository.GetCategory(id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await this.repository.GetCategories();
        }

        /// <inheritdoc/>
        public async Task AddCategory(Category category)
        {
            if (category.ParentCategoryId is not null)
            {
                var parentCategory = await this.repository.GetCategory(category.ParentCategoryId.Value);

                if (parentCategory is null)
                {
                    throw new InvalidCategoryException();
                }
            }

            await this.repository.AddCategory(category);
        }

        /// <inheritdoc/>
        public async Task RemoveCategory(int id)
        {
            await this.repository.RemoveCategory(id);
        }

        /// <inheritdoc/>
        public async Task UpdateCategory(Category category)
        {
            if (category.ParentCategoryId is not null)
            {
                var parentCategory = await this.repository.GetCategory(category.ParentCategoryId.Value);

                if (parentCategory is null)
                {
                    throw new InvalidCategoryException();
                }
            }

            await this.repository.UpdateCategory(category);
        }
    }
}
