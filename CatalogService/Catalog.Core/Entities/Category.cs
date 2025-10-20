using System.ComponentModel.DataAnnotations;
using Catalog.Core.Entities.ValueObjects;

namespace Catalog.Core.Entities
{
    public class Category
    {
        public int Id { get; set; }

        public Name Name { get; set; }

        [Url]
        public string? Image { get; set; }

        public int? ParentCategoryId { get; set; }

        public Category(string name, string? image = null, int? parentCategoryId = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new InvalidOperationException("Category Name is required.");
            }

            this.Name = new Name(name);
            this.Image = image;
            this.ParentCategoryId = parentCategoryId;
        }

        private Category() {}
    }
}
