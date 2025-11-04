namespace CartApp.WebApi.Dtos
{
    /// <summary>
    /// Represents a response DTO model of an item in a shopping cart.
    /// </summary>
    public class ResponseCartItem
    {
        /// <summary>
        /// Gets or sets the id of the cart item.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
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
        public decimal? Price { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the item.
        /// </summary>
        public int? Quantity { get; set; }
    }
}
