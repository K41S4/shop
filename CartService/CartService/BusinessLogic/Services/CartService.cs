using CartApp.Models;
using CartApp.Persistence.Repositories;

namespace CartApp.BusinessLogic.Services
{
    public class CartService(ICartRepository repository) : ICartService
    {
        public void AddCart(Cart cart)
        {
            repository.CreateCart(cart);
        }

        public IEnumerable<CartItem> GetCartItems(int cartId)
        {
            var cart = repository.GetCart(cartId);

            return cart.Items;
        }

        public void AddItemToCart(CartItem item, int cartId)
        {
            var cart = repository.GetCart(cartId);

            var cartItem = cart.Items.FirstOrDefault(x => x.Id == item.Id);
            if (cartItem is not null)
            {
                cartItem.IncrementQuantity();
            }
            else
            {
                cart.Items.Add(item);
            }

            repository.SaveCart(cart);
        }

        public void RemoveItemFromCart(CartItem item, int cartId)
        {
            var cart = repository.GetCart(cartId);

            var cartItem = cart.Items.FirstOrDefault(x => x.Id == item.Id);

            if (cartItem is null)
            {
                return;
            }

            if (cartItem.Quantity > 1)
            {
                cartItem.DecrementQuantity();
            }
            else
            {
                cart.Items.Remove(cartItem);
            }

            repository.SaveCart(cart);
        }
    }
}
