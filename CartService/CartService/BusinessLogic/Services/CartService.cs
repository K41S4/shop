using CartApp.Models;
using CartApp.Persistence.Repositories;

namespace CartApp.BusinessLogic.Services
{
    /// <summary>
    /// Service for managing cart operations.
    /// </summary>
    public class CartService(ICartRepository repository) : ICartService
    {
        /// <inheritdoc/>
        public async Task AddCart(Cart cart)
        {
            await repository.CreateCart(cart);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CartItem>?> GetCartItems(int cartId)
        {
            var cart = await repository.GetCart(cartId);

            return cart.Items;
        }

        /// <inheritdoc/>
        public async Task AddItemToCart(CartItem item, int cartId)
        {
            var cart = await repository.GetCart(cartId);

            var cartItem = cart.Items?.FirstOrDefault(x => x.Id == item.Id);
            if (cartItem is not null)
            {
                cartItem.Quantity += item.Quantity;
            }
            else
            {
                cart.Items?.Add(item);
            }

            await repository.SaveCart(cart);
        }

        /// <inheritdoc/>
        public async Task RemoveItemFromCart(CartItem item, int cartId)
        {
            var cart = await repository.GetCart(cartId);

            var cartItem = cart.Items?.FirstOrDefault(x => x.Id == item.Id);

            if (cartItem is null)
            {
                return;
            }

            if (cartItem.Quantity - item.Quantity > 0)
            {
                cartItem.Quantity -= item.Quantity;
            }
            else
            {
                cart.Items?.Remove(cartItem);
            }

            await repository.SaveCart(cart);
        }
    }
}
