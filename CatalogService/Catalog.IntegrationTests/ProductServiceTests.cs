using AutoMapper;
using Catalog.Core.Entities;
using Catalog.Core.Services;
using Catalog.Persistence.DBContext;
using Catalog.Persistence.MappingProfiles;
using Catalog.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Catalog.IntegrationTests
{
    public class ProductServiceTests : IDisposable
    {
        private readonly CatalogDBContext dbContext;
        private readonly ProductService productService;

        public ProductServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductProfile>();
                cfg.AddProfile<CategoryProfile>();
            }, new LoggerFactory());

            var mapper = config.CreateMapper();

            var dbOptions = new DbContextOptionsBuilder<CatalogDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            dbContext = new CatalogDBContext(dbOptions);

            var productRepository = new ProductRepository(dbContext, mapper);

            productService = new ProductService(productRepository);
        }

        [Fact]
        public async Task AddProduct_ShouldAddProductToDatabase()
        {
            // Arrange
            var product = new Product("Test Product", 24, 10, 5);

            // Act
            await productService.AddProduct(product);

            // Assert
            var dbProduct = await dbContext.Products.FirstOrDefaultAsync();
            Assert.NotNull(dbProduct);
            Assert.Equal("Test Product", dbProduct.Name);
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
