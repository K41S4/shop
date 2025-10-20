using CartApp.Models;

namespace CartApp.BusinessLogic.Services
{
    public interface ICartService
    {
        public void AddCart(Cart cart);
        public IEnumerable<CartItem> GetCartItems(int cartId);
        public void AddItemToCart(CartItem item, int cartId);
        public void RemoveItemFromCart(CartItem item, int cartId);
    }
}
