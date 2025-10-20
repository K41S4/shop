using CartApp.BusinessLogic.Services;
using CartApp.Models;
using CartApp.Persistence.Repositories;

namespace CartApp.IntegrationTests
{
    public class CartServiceIntegrationTests : IDisposable
    {
        private readonly string testDatabasePath;
        private readonly ICartRepository cartRepository;
        private readonly ICartService cartService;

        public CartServiceIntegrationTests()
        {
            testDatabasePath = Path.Combine(Path.GetTempPath(), $"test_cart_{Guid.NewGuid()}.db");
            cartRepository = new CartRepository(testDatabasePath);
            cartService = new CartService(cartRepository);
        }

        public void Dispose()
        {
            if (File.Exists(testDatabasePath))
            {
                File.Delete(testDatabasePath);
            }
        }

        [Fact]
        public void AddCart_WithValidCart_ShouldSucceed()
        {
            // Arrange
            var cart = new Cart
            {
                Id = 1,
                Items = new List<CartItem>()
            };

            // Act
            cartService.AddCart(cart);

            // Assert
            var retrievedCart = cartRepository.GetCart(1);
            Assert.NotNull(retrievedCart);
            Assert.Equal(1, retrievedCart.Id);
        }

        [Fact]
        public void GetCartItems_WithExistingCart_ShouldReturnItems()
        {
            // Arrange
            var cart = new Cart
            {
                Id = 1,
                Items = new List<CartItem>()
            };
            var item1 = new CartItem(1, "Item 1", 10.99m, 2);
            var item2 = new CartItem(2, "Item 2", 5.50m, 1);
            cart.Items.Add(item1);
            cart.Items.Add(item2);
            cartRepository.CreateCart(cart);

            // Act
            var items = cartService.GetCartItems(1);

            // Assert
            Assert.NotNull(items);
            Assert.Equal(2, items.Count());
            Assert.Contains(items, item => item is { Id: 1, Name: "Item 1" });
            Assert.Contains(items, item => item is { Id: 2, Name: "Item 2" });
        }

        [Fact]
        public void AddItemToCart_WithExistingItem_ShouldIncrementQuantity()
        {
            // Arrange
            var cart = new Cart
            {
                Id = 1,
                Items = new List<CartItem>()
            };
            var existingItem = new CartItem(1, "Existing Item", 10.99m, 2);
            cart.Items.Add(existingItem);
            cartRepository.CreateCart(cart);

            var itemToAdd = new CartItem(1, "Existing Item", 10.99m, 1);

            // Act
            cartService.AddItemToCart(itemToAdd, 1);

            // Assert
            var retrievedCart = cartRepository.GetCart(1);
            Assert.NotNull(retrievedCart);
            Assert.Single(retrievedCart.Items);
            var updatedItem = retrievedCart.Items.First();
            Assert.Equal(3, updatedItem.Quantity);
        }
    }
}
