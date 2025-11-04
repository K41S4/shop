using CartApp.BusinessLogic.Services;
using CartApp.Models;
using CartApp.Persistence.Repositories;
using FluentAssertions;
using LiteDB.Async;

namespace CartApp.IntegrationTests.Services
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
        /// Tests that GetCartItems returns items for an existing cart.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task GetCartItems_WithExistingCart_ShouldReturnItems()
        {
            // Arrange
            const string cartId = "1";
            var cart = new Cart
            {
                Id = cartId,
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
            var items = await this.cartService.GetCartItems(cartId);

            // Assert
            items.Should().BeEquivalentTo(cart.Items);
        }

        /// <summary>
        /// Tests that AddItemToCart increments quantity for existing items.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task AddItemToCart_WithExistingItem_ShouldIncrementQuantity()
        {
            // Arrange
            const string cartId = "1";
            var cart = new Cart
            {
                Id = cartId,
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

            var expectedCart = new Cart
            {
                Id = cartId,
                Items = new List<CartItem>
                {
                    new ()
                    {
                        Id = 1,
                        Name = "Existing Item",
                        Price = 10.99m,
                        Quantity = 3,
                    },
                },
            };

            // Act
            await this.cartService.AddItemToCart(cartId, itemToAdd);

            // Assert
            var retrievedCart = await this.cartRepository.GetCart(cartId);
            retrievedCart.Should().BeEquivalentTo(expectedCart);
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
