namespace Catalog.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a price is negative.
    /// </summary>
    public class NegativePriceException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NegativePriceException"/> class.
        /// </summary>
        public NegativePriceException()
            : base("Price cannot be negative.")
        {
        }
    }
}
