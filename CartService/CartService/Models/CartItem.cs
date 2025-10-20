namespace CartApp.Models
{
    public class CartItem
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string? ImageUrl { get; private set; }
        public string? ImageDescription { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }

        public CartItem() {}

        public CartItem(int id, string name, decimal price, int quantity, string? imageUrl = null,
            string? imageDescription = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Item name is required");
            }

            if (price < 0)
            {
                throw new ArgumentException("Price cannot be negative");
            }

            if (quantity < 0)
            {
                throw new ArgumentException("Quantity cannot be negative");
            }

            this.Id = id;
            this.Name = name;
            this.Price = price;
            this.Quantity = quantity;
            this.ImageUrl = imageUrl;
            this.ImageDescription = imageDescription;
        }

        public void IncrementQuantity()
        {
            this.Quantity++;
        }

        public void DecrementQuantity()
        {
            if (this.Quantity > 0)
            {
                this.Quantity--;
            }
            else
            {
                throw new InvalidOperationException("Quantity cannot be negative");
            }
        }
    }
}
