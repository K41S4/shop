using CartApp.Models;

namespace CartApp.BusinessLogic.Services
{
    /// <summary>
    /// Defines the interface for cart service operations.
    /// </summary>
    public interface ICartService
    {
        /// <summary>
        /// Gets a cart.
        /// </summary>
        /// <param name="cartId">The cart id.</param>
        /// <returns>The found cart.</returns>
        public Task<Cart?> GetCart(string cartId);

        /// <summary>
        /// Gets all items in a cart.
        /// </summary>
        /// <param name="cartId">The cart id.</param>
        /// <returns>A collection of cart items if found, otherwise null.</returns>
        public Task<List<CartItem>> GetCartItems(string cartId);

        /// <summary>
        /// Adds an item to a cart.
        /// </summary>
        /// <param name="cartId">The cart id.</param>
        /// <param name="item">The item to add.</param>
        /// <returns>Task.</returns>
        public Task AddItemToCart(string cartId, CartItem item);

        /// <summary>
        /// Removes an item from a cart.
        /// </summary>
        /// <param name="cartId">The cart id.</param>
        /// <param name="itemId">The item id to remove.</param>
        /// <returns>Task.</returns>
        public Task RemoveItemFromCart(string cartId, int itemId);

        /// <summary>
        /// Updates a cart item when product information changes.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <param name="name">The new product name.</param>
        /// <param name="price">The new product price.</param>
        /// <param name="imageUrl">The new product image URL.</param>
        /// <returns>Task.</returns>
        public Task UpdateCartItem(int productId, string name, decimal price, string? imageUrl);
    }
}
