using CartApp.Models;

namespace CartApp.Persistence.Repositories
{
    /// <summary>
    /// Defines the interface for cart repository.
    /// </summary>
    public interface ICartRepository
    {
        /// <summary>
        /// Creates a new cart.
        /// </summary>
        /// <param name="cart">The cart to create.</param>
        /// <returns>Task.</returns>
        public Task CreateCart(Cart cart);

        /// <summary>
        /// Gets a cart by its id.
        /// </summary>
        /// <param name="cartId">The cart id.</param>
        /// <returns>The cart if found.</returns>
        public Task<Cart> GetCart(int cartId);

        /// <summary>
        /// Saves a cart.
        /// </summary>
        /// <param name="cart">The cart to save.</param>
        /// <returns>Task.</returns>
        public Task SaveCart(Cart cart);
    }
}
