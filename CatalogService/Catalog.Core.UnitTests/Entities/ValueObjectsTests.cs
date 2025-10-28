using Catalog.Core.Entities.ValueObjects;
using Catalog.Core.Exceptions;

namespace Catalog.Core.UnitTests.Entities
{
    /// <summary>
    /// Unit tests for value objects validation.
    /// </summary>
    public class ValueObjectsTests
    {
        /// <summary>
        /// Tests that Name throws exception when value is empty.
        /// </summary>
        [Fact]
        public void Name_EmptyName_ThrowsException()
        {
            var ex = Assert.Throws<InvalidNameException>(() =>
            {
                new Name { Value = string.Empty };
            });

            Assert.Equal("Name is invalid.", ex.Message);
        }

        /// <summary>
        /// Tests that Name throws exception when value is longer than 50 characters.
        /// </summary>
        [Fact]
        public void Name_NameLongerThan50_ThrowsException()
        {
            var ex = Assert.Throws<InvalidNameException>(() =>
            {
                new Name { Value = "1234567891234567891234567891234567891234567891234567890" };
            });

            Assert.Equal("Name is invalid.", ex.Message);
        }

        /// <summary>
        /// Tests that Amount throws exception when value is negative.
        /// </summary>
        [Fact]
        public void Amount_Negative_ThrowsException()
        {
            var ex = Assert.Throws<NegativeAmountException>(() =>
            {
                new Amount { Value = -1 };
            });

            Assert.Equal("Amount cannot be less than zero.", ex.Message);
        }

        /// <summary>
        /// Tests that Price throws exception when value is negative.
        /// </summary>
        [Fact]
        public void Price_Negative_ThrowsException()
        {
            var ex = Assert.Throws<NegativePriceException>(() =>
            {
                new Price { Value = -1 };
            });

            Assert.Equal("Price cannot be negative.", ex.Message);
        }

        /// <summary>
        /// Tests that Url throws exception when value is invalid.
        /// </summary>
        [Fact]
        public void Url_InvalidUrl_ThrowsException()
        {
            var ex = Assert.Throws<InvalidUrlException>(() =>
            {
                new Url { Value = "123456789" };
            });

            Assert.Equal("Url is invalid.", ex.Message);
        }
    }
}
