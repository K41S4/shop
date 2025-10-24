using CartApp.BusinessLogic.Services;
using CartApp.Models;
using CartApp.Persistence.Repositories;
using LiteDB.Async;

namespace CartApp.IntegrationTests
{
    /// <summary>
    /// Integration tests for the CartService.
    /// </summary>
    public class CartServiceIntegrationTests : IDisposable
    {
        private readonly string testDatabasePath;
        private readonly ICartRepository cartRepository;
        private readonly ICartService cartService;
        private readonly ILiteDatabaseAsync liteDb;
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartServiceIntegrationTests"/> class.
        /// </summary>
        public CartServiceIntegrationTests()
        {
            this.testDatabasePath = Path.Combine(Path.GetTempPath(), $"test_cart_{Guid.NewGuid()}.db");
            this.liteDb = new LiteDatabaseAsync(this.testDatabasePath);
            this.cartRepository = new CartRepository(this.liteDb);
            this.cartService = new CartService(this.cartRepository);
        }

        /// <summary>
        /// Tests that AddCart succeeds with a valid cart.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task AddCart_WithValidCart_ShouldSucceed()
        {
            // Arrange
            var cart = new Cart
            {
                Id = 1,
                Items = new List<CartItem>(),
            };

            // Act
            await this.cartService.AddCart(cart);

            // Assert
            var retrievedCart = await this.cartRepository.GetCart(1);
            Assert.NotNull(retrievedCart);
            Assert.Equal(1, retrievedCart.Id);
        }

        /// <summary>
        /// Tests that GetCartItems returns items for an existing cart.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task GetCartItems_WithExistingCart_ShouldReturnItems()
        {
            // Arrange
            var cart = new Cart
            {
                Id = 1,
                Items = new List<CartItem>(),
            };
            var item1 = new CartItem
            {
                Id = 1,
                Name = "Item 1",
                Price = 10.99m,
                Quantity = 2,
            };
            var item2 = new CartItem
            {
                Id = 2,
                Name = "Item 2",
                Price = 5.50m,
                Quantity = 1,
            };
            cart.Items.Add(item1);
            cart.Items.Add(item2);
            await this.cartRepository.CreateCart(cart);

            // Act
            var items = await this.cartService.GetCartItems(1);

            // Assert
            Assert.NotNull(items);
            Assert.Equal(2, items.Count());
            Assert.Contains(items, item => item is { Id: 1, Name: "Item 1" });
            Assert.Contains(items, item => item is { Id: 2, Name: "Item 2" });
        }

        /// <summary>
        /// Tests that AddItemToCart increments quantity for existing items.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task AddItemToCart_WithExistingItem_ShouldIncrementQuantity()
        {
            // Arrange
            var cart = new Cart
            {
                Id = 1,
                Items = new List<CartItem>(),
            };
            var existingItem = new CartItem
            {
                Id = 1,
                Name = "Existing Item",
                Price = 10.99m,
                Quantity = 2,
            };
            cart.Items.Add(existingItem);
            await this.cartRepository.CreateCart(cart);

            var itemToAdd = new CartItem
            {
                Id = 1,
                Name = "Existing Item",
                Price = 10.99m,
                Quantity = 1,
            };

            // Act
            await this.cartService.AddItemToCart(itemToAdd, 1);

            // Assert
            var retrievedCart = await this.cartRepository.GetCart(1);
            Assert.NotNull(retrievedCart);
            Assert.Single(retrievedCart.Items!);
            var updatedItem = retrievedCart.Items!.First();
            Assert.Equal(3, updatedItem.Quantity);
        }

        /// <summary>
        /// Disposes the test resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
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
                    this.liteDb.Dispose();
                    if (File.Exists(this.testDatabasePath))
                    {
                        File.Delete(this.testDatabasePath);
                    }
                }

                this.disposed = true;
            }
        }
    }
}
