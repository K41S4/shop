namespace CartApp.BusinessLogic.Exceptions
{
    /// <summary>
    /// Exception thrown when a quantity is negative.
    /// </summary>
    public class NegativeQuantityException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NegativeQuantityException"/> class.
        /// </summary>
        public NegativeQuantityException()
            : base("Quantity cannot be negative")
        {
        }
    }
}
