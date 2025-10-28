using Catalog.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Persistence.DBContext
{
    /// <summary>
    /// Database context for the catalog application.
    /// </summary>
    public class CatalogDBContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogDBContext"/> class.
        /// </summary>
        public CatalogDBContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogDBContext"/> class.
        /// </summary>
        /// <param name="options">The database context options.</param>
        public CatalogDBContext(DbContextOptions<CatalogDBContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the categories table.
        /// </summary>
        public virtual DbSet<CategoryEntity> Categories { get; set; }

        /// <summary>
        /// Gets or sets the products table.
        /// </summary>
        public virtual DbSet<ProductEntity> Products { get; set; }

        /// <summary>
        /// Configures the database connection.
        /// </summary>
        /// <param name="optionsBuilder">The options' builder.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=catalogappdb;Username=postgres;Password=123456");
            }
        }
    }
}
