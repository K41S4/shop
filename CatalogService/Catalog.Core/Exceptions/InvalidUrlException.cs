namespace Catalog.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a URL is invalid.
    /// </summary>
    public class InvalidUrlException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidUrlException"/> class.
        /// </summary>
        public InvalidUrlException()
            : base("Url is invalid.")
        {
        }
    }
}
