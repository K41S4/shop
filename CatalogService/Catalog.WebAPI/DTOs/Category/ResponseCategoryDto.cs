namespace Catalog.WebAPI.DTOs.Category
{
    /// <summary>
    /// Dto for category response.
    /// </summary>
    public class ResponseCategoryDto
    {
        /// <summary>
        /// Gets or sets the id of the category.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the image URL of the category.
        /// </summary>
        public string? Image { get; set; }

        /// <summary>
        /// Gets or sets the parent category id.
        /// </summary>
        public int? ParentCategoryId { get; set; }
    }
}
