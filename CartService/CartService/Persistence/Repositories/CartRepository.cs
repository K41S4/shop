using CartApp.Models;
using LiteDB;

namespace CartApp.Persistence.Repositories
{
    public class CartRepository : ICartRepository
    {
        private const string CartCollection = "carts";
        private readonly string connectionString;

        public CartRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void CreateCart(Cart cart)
        {
            using var context = new LiteDatabase(this.connectionString);
            var carts = context.GetCollection<Cart>(CartCollection);

            if (carts.FindOne(x => x.Id == cart.Id) is not null)
            {
                throw new ArgumentException("Cart with such ID already exists");
            }

            carts.Insert(cart);
        }

        public Cart GetCart(int cartId)
        {
            using var context = new LiteDatabase(this.connectionString);
            var carts = context.GetCollection<Cart>(CartCollection);

            return carts.FindOne(cart => cart.Id == cartId);
        }

        public void SaveCart(Cart cart)
        {
            using var context = new LiteDatabase(this.connectionString);
            var carts = context.GetCollection<Cart>(CartCollection);
            carts.Update(cart);
        }
    }
}
