﻿namespace Catalog.Persistence.Entities
{
    /// <summary>
    /// Entity representing a product in the database.
    /// </summary>
    public class ProductEntity
    {
        /// <summary>
        /// Gets or sets the id of the product.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
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
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the available amount of the product.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Gets or sets the category id this product belongs to.
        /// </summary>
        public int CategoryId { get; set; }
    }
}
