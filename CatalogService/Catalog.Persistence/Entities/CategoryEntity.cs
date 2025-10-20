namespace Catalog.Persistence.Entities
{
    public class CategoryEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Image { get; set; }

        public int? ParentCategoryId { get; set; }
    }
}
