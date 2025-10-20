using Catalog.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Persistence.DBContext
{
    public class CatalogDBContext : DbContext
    {
        public CatalogDBContext()
        {
        }

        public CatalogDBContext(DbContextOptions<CatalogDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CategoryEntity> Categories { get; set; }

        public virtual DbSet<ProductEntity> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseLazyLoadingProxies();
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=catalogappdb;Username=postgres;Password=123456");
            }
        }
    }
}
