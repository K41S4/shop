using Catalog.Core.Entities;

namespace Catalog.Core.Repositories
{
    /// <summary>
    /// Defines the interface for category repository.
    /// </summary>
    public interface ICategoryRepository
    {
        /// <summary>
        /// Gets a category by its id.
        /// </summary>
        /// <param name="id">The category id.</param>
        /// <returns>The category if found, otherwise null.</returns>
        public Task<Category?> GetCategory(int id);

        /// <summary>
        /// Gets all categories.
        /// </summary>
        /// <returns>A collection of all categories.</returns>
        public Task<IEnumerable<Category>> GetCategories();

        /// <summary>
        /// Adds a new category.
        /// </summary>
        /// <param name="category">The category to add.</param>
        /// <returns>Task.</returns>
        public Task AddCategory(Category category);

        /// <summary>
        /// Removes a category by its id.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <returns>Value indicating whether category was removed or not.</returns>
        public Task<bool> RemoveCategory(int categoryId);

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="category">The category to update.</param>
        /// <returns>Task.</returns>
        public Task UpdateCategory(Category category);
    }
}
