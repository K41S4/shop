using AutoMapper;
using Catalog.Core.Entities;
using Catalog.Core.Entities.ValueObjects;
using Catalog.Core.Services.Interfaces;
using Catalog.WebAPI.Controllers;
using Catalog.WebAPI.DTOs.Product;
using Catalog.WebAPI.Mapping;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Catalog.WebAPI.UnitTests.Controllers
{
    /// <summary>
    /// Unit tests for the ProductsController class.
    /// </summary>
    public class ProductsControllerTests
    {
        private readonly IMapper mapper;
        private readonly Mock<IProductService> mockService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsControllerTests"/> class.
        /// </summary>
        public ProductsControllerTests()
        {
            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.AddProfile<ProductDtosProfile>();
                },
                new NullLoggerFactory());
            this.mapper = config.CreateMapper();

            this.mockService = new Mock<IProductService>();
        }

        /// <summary>
        /// Tests that CreateProduct returns Ok when product is successfully created.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task CreateProduct_WithValidData_ShouldReturnOk()
        {
            // Arrange
            var createDto = new CreateProductDto
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 99.99m,
                Amount = 10,
                CategoryId = 1,
            };

            this.mockService.Setup(s => s.AddProduct(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);

            var controller = new ProductsController(this.mockService.Object, this.mapper);

            // Act
            var result = await controller.CreateProduct(createDto);

            // Assert
            result.Should().BeOfType<OkResult>();
            this.mockService.Verify(s => s.AddProduct(It.IsAny<Product>()), Times.Once);
        }

        /// <summary>
        /// Tests that GetProduct returns Ok with product DTO when product exists.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task GetProduct_WithExistingId_ShouldReturnOk()
        {
            // Arrange
            const int productId = 1;
            var product = new Product
            {
                Id = productId,
                Name = new Name { Value = "Test Product" },
                Description = "Test Description",
                Price = new Price { Value = 99.99m },
                Amount = new Amount { Value = 10 },
                CategoryId = 1,
            };

            this.mockService.Setup(s => s.GetProduct(productId))
                .ReturnsAsync(product);

            var controller = new ProductsController(this.mockService.Object, this.mapper);

            // Act
            var result = await controller.GetProduct(productId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult?.Value.Should().BeOfType<ResponseProductDto>();
            var productDto = okResult?.Value as ResponseProductDto;
            productDto?.Id.Should().Be(productId);
            productDto?.Name.Should().Be("Test Product");
        }
    }
}