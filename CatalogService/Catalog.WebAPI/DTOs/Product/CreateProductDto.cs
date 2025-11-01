using System.ComponentModel.DataAnnotations;

namespace Catalog.WebAPI.DTOs.Product
{
    /// <summary>
    /// Dto for Product creation.
    /// </summary>
    public class CreateProductDto
    {
        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        [Required]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the product.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the image URL of the product.
        /// </summary>
        public string? Image { get; set; }

        /// <summary>
        /// Gets or sets the price of the product.
        /// </summary>
        [Required]
        public decimal? Price { get; set; }

        /// <summary>
        /// Gets or sets the available amount of the product.
        /// </summary>
        [Required]
        public int? Amount { get; set; }

        /// <summary>
        /// Gets or sets the category id this product belongs to.
        /// </summary>
        [Required]
        public int CategoryId { get; set; }
    }
}
