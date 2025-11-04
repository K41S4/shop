namespace CartApp.WebApi.Dtos
{
    /// <summary>
    /// Represents a response DTO model of a shopping cart.
    /// </summary>
    public class ResponseCart
    {
        /// <summary>
        /// Gets or sets the id of the cart.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the list of items in the cart.
        /// </summary>
        public List<ResponseCartItem>? Items { get; set; }
    }
}
