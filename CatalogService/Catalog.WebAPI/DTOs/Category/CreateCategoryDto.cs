using System.ComponentModel.DataAnnotations;

namespace Catalog.WebAPI.DTOs.Category
{
    /// <summary>
    /// Dto for Category creation.
    /// </summary>
    public class CreateCategoryDto
    {
        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        [Required]
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
