namespace Catalog.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a name is invalid.
    /// </summary>
    public class InvalidNameException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidNameException"/> class.
        /// </summary>
        public InvalidNameException()
            : base("Name is invalid.")
        {
        }
    }
}
