using AutoMapper;
using CartApp.BusinessLogic.Exceptions;
using CartApp.BusinessLogic.Services;
using CartApp.Models;
using CartApp.WebApi.Controllers.v1;
using CartApp.WebApi.Dtos;
using CartApp.WebApi.MappingProfiles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Shouldly;

namespace CartApp.UnitTests.WebApi.Controllers.v1
{
    /// <summary>
    /// Unit tests for the CartsV1Controller class.
    /// </summary>
    public class CartsV1ControllerTests
    {
        private readonly IMapper mapper;
        private readonly Mock<ICartService> mockService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartsV1ControllerTests"/> class.
        /// </summary>
        public CartsV1ControllerTests()
        {
            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.AddProfile<CartItemMappingProfile>();
                    cfg.AddProfile<CartMappingProfile>();
                },
                new NullLoggerFactory());
            this.mapper = config.CreateMapper();

            this.mockService = new Mock<ICartService>();
        }

        /// <summary>
        /// Tests that GetCartInfo returns NotFound when cart is null.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task GetCartInfo_CartNotFound_ReturnsNotFound()
        {
            // Arrange
            const string cartId = "non-existent-id";
            this.mockService.Setup(s => s.GetCart(cartId)).ReturnsAsync((Cart?)null);

            var controller = new CartsV1Controller(this.mockService.Object, this.mapper);

            // Act
            var result = await controller.GetCartInfo(cartId);

            // Assert
            result.ShouldBeOfType<NotFoundObjectResult>();
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult?.Value.ShouldBe($"Cart with {cartId} ID was not found.");
        }

        /// <summary>
        /// Tests that GetCartInfo returns Ok with cart DTO when cart exists.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task GetCartInfo_CartExists_ReturnsOk()
        {
            // Arrange
            const string cartId = "test-cart-id";
            var cart = new Cart
            {
                Id = cartId,
                Items = new List<CartItem>
                {
                    new ()
                    {
                        Id = 1,
                        Name = "Test Item",
                        Price = 10.50m,
                        Quantity = 2,
                    },
                },
            };

            this.mockService.Setup(s => s.GetCart(cartId)).ReturnsAsync(cart);

            var controller = new CartsV1Controller(this.mockService.Object, this.mapper);

            // Act
            var result = await controller.GetCartInfo(cartId);

            // Assert
            result.ShouldBeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult?.Value.ShouldBeOfType<ResponseCart>();
            var cartDto = okResult?.Value as ResponseCart;
            cartDto?.Id.ShouldBe(cartId);
            cartDto?.Items.ShouldNotBeNull();
            cartDto?.Items?.Count.ShouldBe(1);
        }

        /// <summary>
        /// Tests that DeleteItemFromCart returns NotFound when NotFoundException is thrown.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task DeleteItemFromCart_CartNotFound_ReturnsNotFound()
        {
            // Arrange
            const string cartId = "non-existent-id";
            const int itemId = 1;

            this.mockService.Setup(s => s.RemoveItemFromCart(cartId, itemId))
                .ThrowsAsync(new NotFoundException($"Cart with {cartId} ID was not found."));

            var controller = new CartsV1Controller(this.mockService.Object, this.mapper);

            // Act
            var result = await controller.DeleteItemFromCart(cartId, itemId);

            // Assert
            result.ShouldBeOfType<NotFoundObjectResult>();
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult?.Value.ShouldNotBeNull();
        }

        /// <summary>
        /// Tests that DeleteItemFromCart returns NotFound when item is not found.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task DeleteItemFromCart_ItemNotFound_ReturnsNotFound()
        {
            // Arrange
            const string cartId = "test-cart-id";
            const int itemId = 999;

            this.mockService.Setup(s => s.RemoveItemFromCart(cartId, itemId))
                .ThrowsAsync(new NotFoundException($"Cart item with {itemId} ID was not found."));

            var controller = new CartsV1Controller(this.mockService.Object, this.mapper);

            // Act
            var result = await controller.DeleteItemFromCart(cartId, itemId);

            // Assert
            result.ShouldBeOfType<NotFoundObjectResult>();
        }

        /// <summary>
        /// Tests that DeleteItemFromCart returns Ok when item is successfully deleted.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task DeleteItemFromCart_Success_ReturnsOk()
        {
            // Arrange
            const string cartId = "test-cart-id";
            const int itemId = 1;

            this.mockService.Setup(s => s.RemoveItemFromCart(cartId, itemId)).Returns(Task.CompletedTask);

            var controller = new CartsV1Controller(this.mockService.Object, this.mapper);

            // Act
            var result = await controller.DeleteItemFromCart(cartId, itemId);

            // Assert
            result.ShouldBeOfType<OkResult>();
        }

        /// <summary>
        /// Tests that AddItemToCart returns Ok when item is successfully added.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task AddItemToCart_Success_ReturnsOk()
        {
            // Arrange
            const string cartId = "test-cart-id";
            var cartItemDto = new AddCartItem
            {
                Id = 1,
                Name = "Test Item",
                Price = 10.50m,
                Quantity = 1,
            };

            this.mockService.Setup(s => s.AddItemToCart(cartId, It.IsAny<CartItem>()))
                .Returns(Task.CompletedTask);

            var controller = new CartsV1Controller(this.mockService.Object, this.mapper);

            // Act
            var result = await controller.AddItemToCart(cartId, cartItemDto);

            // Assert
            result.ShouldBeOfType<OkResult>();
            this.mockService.Verify(s => s.AddItemToCart(cartId, It.IsAny<CartItem>()), Times.Once);
        }
    }
}
