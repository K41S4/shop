using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Services;
using Moq;

namespace Catalog.Core.UnitTests
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> mockRepository;
        private readonly ProductService productService;

        public ProductServiceTests()
        {
            mockRepository = new Mock<IProductRepository>();
            productService = new ProductService(mockRepository.Object);
        }

        [Fact]
        public async Task GetProducts_ShouldGetAllProducts()
        {
            // Arrange
            var product = new Product("Test Product", 24, 10, 5);
            this.mockRepository.Setup(x => x.GetProducts()).ReturnsAsync(new List<Product> { product });

            // Act
            var result = await productService.GetProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count());
        }
    }
}
