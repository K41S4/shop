using AutoMapper;
using Catalog.Core.Entities;
using Catalog.Core.Entities.ValueObjects;
using Catalog.Core.Services;
using Catalog.Persistence.DBContext;
using Catalog.Persistence.Entities;
using Catalog.Persistence.MappingProfiles;
using Catalog.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Catalog.IntegrationTests
{
    /// <summary>
    /// Tests for Product Service.
    /// </summary>
    public class ProductServiceTests : IDisposable
    {
        private readonly CatalogDBContext dbContext;
        private readonly ProductService productService;
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductServiceTests"/> class.
        /// </summary>
        public ProductServiceTests()
        {
            var config = new MapperConfiguration(
                cfg =>
            {
                cfg.AddProfile<ProductProfile>();
                cfg.AddProfile<CategoryProfile>();
            }, new LoggerFactory());

            var mapper = config.CreateMapper();

            var dbOptions = new DbContextOptionsBuilder<CatalogDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            this.dbContext = new CatalogDBContext(dbOptions);

            var productRepository = new ProductRepository(this.dbContext, mapper);
            var categoryRepository = new CategoryRepository(this.dbContext, mapper);

            this.productService = new ProductService(productRepository, categoryRepository);
        }

        /// <summary>
        /// Tests that AddProduct adds correct product to database when category exists.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task AddProduct_CategoryExists_ShouldAddProductToDatabase()
        {
            // Arrange
            var category = new CategoryEntity
            {
                Id = 1,
                Name = "Test Category",
            };
            await this.dbContext.Categories.AddAsync(category);
            await this.dbContext.SaveChangesAsync();

            var product = new Product
            {
                Name = new Name { Value = "Test product" },
                Amount = new Amount { Value = 1 },
                CategoryId = 1,
                Price = new Price { Value = 1.1m },
            };

            // Act
            await this.productService.AddProduct(product);

            // Assert
            var dbProduct = await this.dbContext.Products.FirstOrDefaultAsync();
            dbProduct.Should().BeEquivalentTo(new
            {
                Name = product.Name.Value,
                Amount = product.Amount.Value,
                Price = product.Price.Value,
                product.CategoryId,
            });
        }

        /// <summary>
        /// Disposes the test resources.
        /// </summary>
        /// <param name="disposing">Whether disposing is in progress.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.dbContext.Dispose();
                }

                this.disposed = true;
            }
        }

        /// <summary>
        /// Disposes the test resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
