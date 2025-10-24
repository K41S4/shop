using Catalog.Core.Entities.ValueObjects;

namespace Catalog.Core.Entities
{
    /// <summary>
    /// Represents a category in the catalog.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Gets or sets the id of the category.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        public required Name Name { get; set; }

        /// <summary>
        /// Gets or sets the image URL of the category.
        /// </summary>
        public Url? Image { get; set; }

        /// <summary>
        /// Gets or sets the parent category id. Null for root categories.
        /// </summary>
        public int? ParentCategoryId { get; set; }
    }
}
