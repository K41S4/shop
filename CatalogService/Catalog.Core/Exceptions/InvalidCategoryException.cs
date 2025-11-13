namespace Catalog.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a category does not exist.
    /// </summary>
    public class InvalidCategoryException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCategoryException"/> class.
        /// </summary>
        public InvalidCategoryException()
            : base("Category does not exist.")
        {
        }
    }
}
