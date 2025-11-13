using CartApp.BusinessLogic.Exceptions;
using CartApp.BusinessLogic.Services;
using CartApp.Models;
using CartApp.WebApi.Controllers.v2;
using CartApp.WebApi.Dtos;
using CartApp.WebApi.MappingProfiles;
using FluentAssertions;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;

namespace CartApp.UnitTests.WebApi.Controllers.v2
{
    /// <summary>
    /// Unit tests for the CartsV2Controller class.
    /// </summary>
    public class CartsV2ControllerTests
    {
        private readonly IMapper mapper;
        private readonly Mock<ICartService> mockService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartsV2ControllerTests"/> class.
        /// </summary>
        public CartsV2ControllerTests()
        {
            var config = new TypeAdapterConfig();
            config.Scan(typeof(CartItemMappingConfig).Assembly);
            this.mapper = new Mapper(config);

            this.mockService = new Mock<ICartService>();
        }

        /// <summary>
        /// Tests that GetCartInfo pushes NotFoundException further when it is thrown.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task GetCartInfo_CartNotFound_ReturnsNotFound()
        {
            // Arrange
            const string cartId = "non-existent-id";

            this.mockService.Setup(s => s.GetCartItems(cartId))
                .ThrowsAsync(new NotFoundException($"Cart with {cartId} ID was not found."));

            var controller = new CartsV2Controller(this.mockService.Object, this.mapper);

            // Act
            // Assert
            var exception =
                await Assert.ThrowsAsync<NotFoundException>(async () => await controller.GetCartInfo(cartId));
            exception.Message.ShouldBe("Cart with non-existent-id ID was not found.");
        }

        /// <summary>
        /// Tests that GetCartInfo returns Ok with cart items DTO when cart exists.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task GetCartInfo_CartExists_ReturnsOk()
        {
            // Arrange
            const string cartId = "test-cart-id";
            var cartItems = new List<CartItem>
            {
                new ()
                {
                    Id = 1,
                    Name = "Test Item 1",
                    Price = 10.50m,
                    Quantity = 2,
                },
                new ()
                {
                    Id = 2,
                    Name = "Test Item 2",
                    Price = 20.00m,
                    Quantity = 1,
                },
            };

            this.mockService.Setup(s => s.GetCartItems(cartId)).ReturnsAsync(cartItems);

            var controller = new CartsV2Controller(this.mockService.Object, this.mapper);

            // Act
            var result = await controller.GetCartInfo(cartId);

            // Assert
            result.Should().BeEquivalentTo(new OkObjectResult(cartItems), options =>
                options.ComparingByMembers<ResponseCartItem>());
        }

        /// <summary>
        /// Tests that DeleteItemFromCart pushes NotFoundException further when it is thrown for cart.
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

            var controller = new CartsV2Controller(this.mockService.Object, this.mapper);

            // Act
            // Assert
            var exception =
                await Assert.ThrowsAsync<NotFoundException>(async () => await controller.DeleteItemFromCart(cartId, itemId));
            exception.Message.ShouldBe("Cart with non-existent-id ID was not found.");
        }

        /// <summary>
        /// Tests that DeleteItemFromCart pushes NotFoundException further when it is thrown for cart item.
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

            var controller = new CartsV2Controller(this.mockService.Object, this.mapper);

            // Act
            // Assert
            var exception =
                await Assert.ThrowsAsync<NotFoundException>(async () => await controller.DeleteItemFromCart(cartId, itemId));
            exception.Message.ShouldBe("Cart item with 999 ID was not found.");
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

            var controller = new CartsV2Controller(this.mockService.Object, this.mapper);

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

            var controller = new CartsV2Controller(this.mockService.Object, this.mapper);

            // Act
            var result = await controller.AddItemToCart(cartId, cartItemDto);

            // Assert
            result.ShouldBeOfType<OkResult>();
            this.mockService.Verify(s => s.AddItemToCart(cartId, It.IsAny<CartItem>()), Times.Once);
        }
    }
}
