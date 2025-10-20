using System.ComponentModel.DataAnnotations;
using Catalog.Core.Entities.ValueObjects;

namespace Catalog.Core.Entities
{
    public class Product
    {
        public int Id { get; private set; }

        public Name Name { get; private set; }

        public string? Description { get; private set; }

        [Url]
        public string? Image { get; private set; }

        public Price Price { get; private set; }

        public Amount Amount { get; private set; }
        
        public int CategoryId { get; private set; }

        public Product(string name, int categoryId, decimal price, int amount, string? description = null, string? image = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new InvalidOperationException("Product Name is required.");
            }

            this.Name = new Name(name);
            this.Price = new Price(price);
            this.Amount = new Amount(amount);
            this.CategoryId = categoryId;
            this.Description = description;
            this.Image = image;
        }

        private Product() { }
    }
}
