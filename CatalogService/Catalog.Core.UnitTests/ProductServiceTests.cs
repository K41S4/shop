using Catalog.Core.Entities;
using Catalog.Core.Entities.ValueObjects;
using Catalog.Core.Exceptions;
using Catalog.Core.Repositories;
using Catalog.Core.Services;
using FluentAssertions;
using Moq;

namespace Catalog.Core.UnitTests
{
    /// <summary>
    /// Unit tests for the ProductService class.
    /// </summary>
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> mockProductRepository;
        private readonly Mock<ICategoryRepository> mockCategoryRepository;
        private readonly ProductService productService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductServiceTests"/> class.
        /// </summary>
        public ProductServiceTests()
        {
            this.mockProductRepository = new Mock<IProductRepository>();
            this.mockCategoryRepository = new Mock<ICategoryRepository>();
            this.productService = new ProductService(this.mockProductRepository.Object, this.mockCategoryRepository.Object);
        }

        /// <summary>
        /// Tests that GetProducts returns all products.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task GetProducts_ShouldGetAllProducts()
        {
            // Arrange
            var category = new Category
            {
                Name = new Name { Value = "Test" },
            };
            var product = new Product
            {
                Name = new Name { Value = "Test product" },
                Amount = new Amount { Value = 1 },
                CategoryId = 1,
                Price = new Price { Value = 1.1m },
            };
            this.mockProductRepository.Setup(x => x.GetProducts(1, 1, 1)).ReturnsAsync(new List<Product> { product });
            this.mockCategoryRepository.Setup(x => x.GetCategory(1)).ReturnsAsync(category);

            // Act
            var result = await this.productService.GetProducts(1, 1, 1);

            // Assert
            result.Should().ContainSingle();
            result.First().Should().BeEquivalentTo(product);
        }

        /// <summary>
        /// Tests that AddProduct throws exception when category is invalid.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task AddProduct_InvalidCategory_ShouldThrow()
        {
            // Arrange
            var product = new Product
            {
                Name = new Name { Value = "Test product" },
                Amount = new Amount { Value = 1 },
                CategoryId = 1,
                Price = new Price { Value = 1.1m },
            };

            this.mockCategoryRepository.Setup(x => x.GetCategory(It.IsAny<int>())).ReturnsAsync((Category?)null);

            // Act
            // Assert
            await Assert.ThrowsAsync<InvalidCategoryException>(() => this.productService.AddProduct(product));
            this.mockProductRepository.Verify(x => x.AddProduct(It.IsAny<Product>()), Times.Never);
        }

        /// <summary>
        /// Tests that UpdateProduct throws exception when category is invalid.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task UpdateProduct_InvalidCategory_ShouldThrow()
        {
            // Arrange
            var category = new Category
            {
                Id = 1,
                Name = new Name { Value = "Test" },
            };
            var product = new Product
            {
                Id = 1,
                Name = new Name { Value = "Test product" },
                Amount = new Amount { Value = 1 },
                CategoryId = 2,
                Price = new Price { Value = 1.1m },
            };

            this.mockProductRepository.Setup(x => x.GetProduct(product.Id)).ReturnsAsync(product);
            this.mockCategoryRepository.Setup(x => x.GetCategory(category.Id)).ReturnsAsync(category);

            // Act
            // Assert
            await Assert.ThrowsAsync<InvalidCategoryException>(() => this.productService.UpdateProduct(product));
            this.mockProductRepository.Verify(x => x.UpdateProduct(It.IsAny<Product>()), Times.Never);
        }
    }
}
