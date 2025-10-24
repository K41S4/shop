using Catalog.Core.Exceptions;

namespace Catalog.Core.Entities.ValueObjects
{
    /// <summary>
    /// Represents an amount value object with validation.
    /// </summary>
    public class Amount
    {
        private readonly int amountValue;

        /// <summary>
        /// Gets the amount value. Must not be negative.
        /// </summary>
        public required int Value
        {
            get => this.amountValue;
            init
            {
                if (value < 0)
                {
                    throw new NegativeAmountException();
                }

                this.amountValue = value;
            }
        }
    }
}
