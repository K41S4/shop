using Catalog.Core.Entities.ValueObjects;

namespace Catalog.Core.Entities
{
    /// <summary>
    /// Represents a product in the catalog.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Gets or sets the id of the product.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        public required Name Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the product.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the image URL of the product.
        /// </summary>
        public Url? Image { get; set; }

        /// <summary>
        /// Gets or sets the price of the product.
        /// </summary>
        public required Price Price { get; set; }

        /// <summary>
        /// Gets or sets the available amount of the product.
        /// </summary>
        public required Amount Amount { get; set; }

        /// <summary>
        /// Gets or sets the category id this product belongs to.
        /// </summary>
        public required int CategoryId { get; set; }
    }
}
