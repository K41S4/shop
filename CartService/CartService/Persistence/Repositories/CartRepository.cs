using CartApp.Models;
using LiteDB.Async;

namespace CartApp.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation for cart data access using LiteDB.
    /// </summary>
    public class CartRepository : ICartRepository
    {
        private readonly ILiteDatabaseAsync liteDb;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartRepository"/> class.
        /// </summary>
        /// <param name="liteDb">The LiteDB database instance.</param>
        public CartRepository(ILiteDatabaseAsync liteDb)
        {
            this.liteDb = liteDb;
        }

        /// <inheritdoc/>
        public async Task CreateCart(Cart cart)
        {
            var carts = this.liteDb.GetCollection<Cart>();

            var existingCart = await carts.FindByIdAsync(cart.Id);
            if (existingCart is not null)
            {
                throw new ArgumentException("Cart with such ID already exists");
            }

            await carts.InsertAsync(cart);
        }

        /// <inheritdoc/>
        public async Task<Cart> GetCart(int cartId)
        {
            var carts = this.liteDb.GetCollection<Cart>();

            return await carts.Query().Where(cart => cart.Id == cartId).FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task SaveCart(Cart cart)
        {
            var carts = this.liteDb.GetCollection<Cart>();
            await carts.UpsertAsync(cart);
        }
    }
}
