namespace CartApp.Models
{
    /// <summary>
    /// Represents a shopping cart.
    /// </summary>
    public class Cart
    {
        /// <summary>
        /// Gets or sets the id of the cart.
        /// </summary>
        public required int Id { get; set; }

        /// <summary>
        /// Gets or sets the collection of items in the cart.
        /// </summary>
        public ICollection<CartItem>? Items { get; set; }
    }
}
