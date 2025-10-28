using CartApp.BusinessLogic.Services;
using CartApp.Models;
using CartApp.Persistence.Repositories;
using Moq;
using Shouldly;

namespace CartApp.UnitTests.BusinessLogic.Services
{
    /// <summary>
    /// Unit tests for the CartService class.
    /// </summary>
    public class CartServiceTests
    {
        /// <summary>
        /// Tests that GetCartItems calls the repository.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task GetCartItems_CallsRepository()
        {
            // Arrange
            var cartId = 1;
            var cart = new Cart
            {
                Id = cartId,
                Items = new List<CartItem>
                {
                    new ()
                    {
                        Id = 123,
                        Name = "Product1",
                        Price = 1.1m,
                        Quantity = 1,
                    },
                    new ()
                    {
                        Id = 456,
                        Name = "Product2",
                        Price = 1.1m,
                        Quantity = 1,
                    },
                },
            };

            var mockRepo = new Mock<ICartRepository>();
            mockRepo.Setup(repo => repo.GetCart(cartId)).ReturnsAsync(cart);

            var service = new CartService(mockRepo.Object);

            // Act
            var result = await service.GetCartItems(cartId);

            // Assert
            mockRepo.Verify(repo => repo.GetCart(cartId), Times.Once);
            result.ShouldBeEquivalentTo(cart.Items);
        }

        /// <summary>
        /// Tests that RemoveItemFromCart does not update cart when item does not exist.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task RemoveItemFromCart_ItemDoNotExistInCart_DoesNotUpdateCart()
        {
            // Arrange
            var cartId = 1;
            var cart = new Cart
            {
                Id = cartId,
                Items = new List<CartItem>
                {
                    new ()
                    {
                        Id = 123,
                        Name = "Product1",
                        Price = 1.1m,
                        Quantity = 1,
                    },
                },
            };
            var itemToRemove = new CartItem
            {
                Id = 111,
                Name = "Product1",
                Price = 1.1m,
                Quantity = 1,
            };
            var mockRepo = new Mock<ICartRepository>();
            mockRepo.Setup(repo => repo.GetCart(cartId)).ReturnsAsync(cart);

            var service = new CartService(mockRepo.Object);

            // Act
            await service.RemoveItemFromCart(itemToRemove, cartId);

            // Assert
            mockRepo.Verify(repo => repo.SaveCart(It.IsAny<Cart>()), Times.Never);
        }

        /// <summary>
        /// Tests that RemoveItemFromCart updates cart when item exists.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task RemoveItemFromCart_ItemExistsInCart_UpdatesCart()
        {
            // Arrange
            var cartId = 1;
            var cart = new Cart
            {
                Id = cartId,
                Items = new List<CartItem>
                {
                    new ()
                    {
                        Id = 123,
                        Name = "Product1",
                        Price = 1.1m,
                        Quantity = 1,
                    },
                },
            };
            var itemToRemove = new CartItem
            {
                Id = 123,
                Name = "Product1",
                Price = 1.1m,
                Quantity = 1,
            };
            var mockRepo = new Mock<ICartRepository>();
            mockRepo.Setup(repo => repo.GetCart(cartId)).ReturnsAsync(cart);

            var service = new CartService(mockRepo.Object);

            // Act
            await service.RemoveItemFromCart(itemToRemove, cartId);

            // Assert
            mockRepo.Verify(repo => repo.SaveCart(It.IsAny<Cart>()), Times.Once);
        }
    }
}
