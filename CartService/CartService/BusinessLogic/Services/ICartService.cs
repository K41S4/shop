using CartApp.Models;

namespace CartApp.BusinessLogic.Services
{
    /// <summary>
    /// Defines the interface for cart service operations.
    /// </summary>
    public interface ICartService
    {
        /// <summary>
        /// Adds a new cart.
        /// </summary>
        /// <param name="cart">The cart to add.</param>
        /// <returns>Task.</returns>
        public Task AddCart(Cart cart);

        /// <summary>
        /// Gets all items in a cart.
        /// </summary>
        /// <param name="cartId">The cart id.</param>
        /// <returns>A collection of cart items if found, otherwise null.</returns>
        public Task<IEnumerable<CartItem>?> GetCartItems(int cartId);

        /// <summary>
        /// Adds an item to a cart.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="cartId">The cart id.</param>
        /// <returns>Task.</returns>
        public Task AddItemToCart(CartItem item, int cartId);

        /// <summary>
        /// Removes an item from a cart.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <param name="cartId">The cart id.</param>
        /// <returns>Task.</returns>
        public Task RemoveItemFromCart(CartItem item, int cartId);
    }
}
