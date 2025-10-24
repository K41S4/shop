namespace Catalog.Persistence.Entities
{
    /// <summary>
    /// Entity representing a category in the database.
    /// </summary>
    public class CategoryEntity
    {
        /// <summary>
        /// Gets or sets the id of the category.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the image URL of the category.
        /// </summary>
        public string? Image { get; set; }

        /// <summary>
        /// Gets or sets the parent category id. Null for root categories.
        /// </summary>
        public int? ParentCategoryId { get; set; }
    }
}
