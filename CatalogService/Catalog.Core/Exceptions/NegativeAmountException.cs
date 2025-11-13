namespace Catalog.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when an amount is negative.
    /// </summary>
    public class NegativeAmountException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NegativeAmountException"/> class.
        /// </summary>
        public NegativeAmountException()
            : base("Amount cannot be less than zero.")
        {
        }
    }
}
