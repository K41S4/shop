using Catalog.Core.Exceptions;

namespace Catalog.Core.Entities.ValueObjects
{
    /// <summary>
    /// Represents a price value object with validation.
    /// </summary>
    public class Price
    {
        private readonly decimal priceValue;

        /// <summary>
        /// Gets the price value. Must not be negative.
        /// </summary>
        public required decimal Value
        {
            get => this.priceValue;
            init
            {
                if (value < 0)
                {
                    throw new NegativePriceException();
                }

                this.priceValue = value;
            }
        }
    }
}
