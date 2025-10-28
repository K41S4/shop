using CartApp.Models;
using CartApp.Persistence.Repositories;
using LiteDB.Async;
using Moq;

namespace CartApp.UnitTests.Persistence.Repositories
{
    /// <summary>
    /// Unit tests for the CartRepository class.
    /// </summary>
    public class CartRepositoryTests
    {
        private readonly Mock<ILiteDatabaseAsync> liteDb;
        private readonly ICartRepository cartRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartRepositoryTests"/> class.
        /// </summary>
        public CartRepositoryTests()
        {
            this.liteDb = new Mock<ILiteDatabaseAsync>();
            this.cartRepo = new CartRepository(this.liteDb.Object);
        }

        /// <summary>
        /// Tests that CreateCart throws exception when cart id already exists.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task CreateCart_IdAlreadyExists_Throws()
        {
            // Arrange
            var cart = new Cart
            {
                Id = 1,
            };

            var cartToCreate = new Cart
            {
                Id = 1,
            };

            var collectionMock = new Mock<ILiteCollectionAsync<Cart>>();
            collectionMock.Setup(c => c.FindByIdAsync(cart.Id)).ReturnsAsync(cart);

            this.liteDb.Setup(db => db.GetCollection<Cart>()).Returns(collectionMock.Object);

            // Act
            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await this.cartRepo.CreateCart(cartToCreate);
            });
            Assert.Equal("Cart with such ID already exists", ex.Message);
        }
    }
}
