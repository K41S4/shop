using Catalog.Core.Entities;

namespace Catalog.Core.Services.Interfaces
{
    /// <summary>
    /// Defines the interface for category service.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Gets a category by its identifier.
        /// </summary>
        /// <param name="id">The category identifier.</param>
        /// <returns>The category if found, otherwise null.</returns>
        public Task<Category?> GetCategory(int id);

        /// <summary>
        /// Gets multiple categories by their ids.
        /// </summary>
        /// <param name="ids">The category ids.</param>
        /// <returns>A collection of found categories.</returns>
        public Task<IEnumerable<Category>> GetCategoriesByIds(IReadOnlyList<int> ids);

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
        /// Removes a category by its identifier.
        /// </summary>
        /// <param name="id">The category identifier.</param>
        /// <returns>Task.</returns>
        public Task RemoveCategory(int id);

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="category">The category to update.</param>
        /// <returns>Task.</returns>
        public Task UpdateCategory(Category category);
    }
}
