using CartApp.BusinessLogic.Exceptions;
using CartApp.Models;

namespace CartApp.UnitTests.BusinessLogic.Models
{
    /// <summary>
    /// Unit tests for the CartItem model validation.
    /// </summary>
    public class CartItemTests
    {
        /// <summary>
        /// Tests that CartItem throws exception when name is empty.
        /// </summary>
        [Fact]
        public void CartItem_EmptyName_ThrowsException()
        {
            var ex = Assert.Throws<ItemNameRequiredException>(() =>
            {
                new CartItem
                {
                    Id = 1,
                    Name = string.Empty,
                    Price = 1.1m,
                    Quantity = 1,
                };
            });

            Assert.Equal("Item name is required", ex.Message);
        }

        /// <summary>
        /// Tests that CartItem throws exception when price is negative.
        /// </summary>
        [Fact]
        public void CartItem_NegativePrice_ThrowsException()
        {
            var ex = Assert.Throws<NegativePriceException>(() =>
            {
                new CartItem
                {
                    Id = 1,
                    Name = "test",
                    Price = -1.1m,
                    Quantity = 1,
                };
            });

            Assert.Equal("Price cannot be negative", ex.Message);
        }

        /// <summary>
        /// Tests that CartItem throws exception when quantity is negative.
        /// </summary>
        [Fact]
        public void CartItem_NegativeQuantity_ThrowsException()
        {
            var ex = Assert.Throws<NegativeQuantityException>(() =>
            {
                new CartItem
                {
                    Id = 1,
                    Name = "test",
                    Price = 1.1m,
                    Quantity = -1,
                };
            });

            Assert.Equal("Quantity cannot be negative", ex.Message);
        }
    }
}
