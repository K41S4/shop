using CartApp.BusinessLogic.Exceptions;
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
        public async Task<Cart?> GetCart(string cartId)
        {
            return await repository.GetCart(cartId);
        }

        /// <inheritdoc/>
        public async Task<List<CartItem>> GetCartItems(string cartId)
        {
            var cart = await repository.GetCart(cartId);

            if (cart is null)
            {
                throw new NotFoundException($"Cart with {cartId} ID was not found.");
            }

            return cart.Items.ToList();
        }

        /// <inheritdoc/>
        public async Task AddItemToCart(string cartId, CartItem item)
        {
            var cart = await repository.GetCart(cartId);

            if (cart is null)
            {
                cart = new Cart
                {
                    Id = cartId,
                };
                await repository.CreateCart(cart);
            }

            var cartItem = cart.Items.FirstOrDefault(x => x.Id == item.Id);
            if (cartItem is not null)
            {
                cartItem.Quantity += item.Quantity;
            }
            else
            {
                cart.Items.Add(item);
            }

            await repository.SaveCart(cart);
        }

        /// <inheritdoc/>
        public async Task RemoveItemFromCart(string cartId, int itemId)
        {
            var cart = await repository.GetCart(cartId);

            if (cart is null)
            {
                throw new NotFoundException($"Cart with {cartId} ID was not found.");
            }

            var cartItem = cart.Items.FirstOrDefault(x => x.Id == itemId);

            if (cartItem is null)
            {
                throw new NotFoundException($"Cart item with {itemId} ID was not found.");
            }

            cart.Items.Remove(cartItem);

            await repository.SaveCart(cart);
        }
    }
}
