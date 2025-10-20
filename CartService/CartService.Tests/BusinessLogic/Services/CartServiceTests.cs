using CartApp.BusinessLogic.Services;
using CartApp.Models;
using CartApp.Persistence.Repositories;
using Moq;
using Shouldly;

namespace CartApp.UnitTests.BusinessLogic.Services
{
    public class CartServiceTests
    {
        [Fact]
        public void GetCartItems_CallsRepository()
        {
            // Arrange
            var cartId = 1;
            var cart = new Cart
            {
                Id = cartId,
                Items = new List<CartItem>
                {
                    new(123, "Product1", 1.1m, 1),
                    new(456, "Product2", 1.1m, 1),

                }
            };

            var mockRepo = new Mock<ICartRepository>();
            mockRepo.Setup(repo => repo.GetCart(cartId)).Returns(cart);

            var service = new CartService(mockRepo.Object);

            // Act
            var result = service.GetCartItems(cartId);

            // Assert
            mockRepo.Verify(repo => repo.GetCart(cartId), Times.Once);
            result.ShouldBeEquivalentTo(cart.Items);
        }
    }
}
