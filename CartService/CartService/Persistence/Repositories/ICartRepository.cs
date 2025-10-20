using CartApp.Models;

namespace CartApp.Persistence.Repositories
{
    public interface ICartRepository
    {
        public void CreateCart(Models.Cart cart);
        public Models.Cart GetCart(int cartId);
        public void SaveCart(Models.Cart cart);
    }
}
