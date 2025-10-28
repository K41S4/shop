using CartApp.BusinessLogic.Exceptions;

namespace CartApp.Models
{
    /// <summary>
    /// Represents an item in a shopping cart.
    /// </summary>
    public class CartItem
    {
        private readonly string? name;
        private readonly decimal price;
        private int quantity;

        /// <summary>
        /// Gets or sets the id of the cart item.
        /// </summary>
        public required int Id { get; set; }

        /// <summary>
        /// Gets the name of the item.
        /// </summary>
        public required string? Name
        {
            get => this.name;
            init
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ItemNameRequiredException();
                }

                this.name = value;
            }
        }

        /// <summary>
        /// Gets or sets the image URL of the item.
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the image description of the item.
        /// </summary>
        public string? ImageDescription { get; set; }

        /// <summary>
        /// Gets the price of the item.
        /// </summary>
        public required decimal Price
        {
            get => this.price;
            init
            {
                if (value < 0)
                {
                    throw new NegativePriceException();
                }

                this.price = value;
            }
        }

        /// <summary>
        /// Gets or sets the quantity of the item.
        /// </summary>
        public int Quantity
        {
            get => this.quantity;
            set
            {
                if (value < 0)
                {
                    throw new NegativeQuantityException();
                }

                this.quantity = value;
            }
        }
    }
}
