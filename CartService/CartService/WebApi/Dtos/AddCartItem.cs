using System.ComponentModel.DataAnnotations;

namespace CartApp.WebApi.Dtos
{
    /// <summary>
    /// Represents a DTO model for adding an item to a shopping cart.
    /// </summary>
    public class AddCartItem
    {
        /// <summary>
        /// Gets or sets the id of the cart item.
        /// </summary>
        [Required]
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        [Required]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the image URL of the item.
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the image description of the item.
        /// </summary>
        public string? ImageDescription { get; set; }

        /// <summary>
        /// Gets or sets the price of the item.
        /// </summary>
        [Required]
        public decimal? Price { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the item.
        /// </summary>
        [Required]
        public int? Quantity { get; set; }
    }
}
