namespace Catalog.Core.Exceptions
{
    /// <summary>
    /// Not found exception.
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public NotFoundException(string message)
            : base(message)
        {
        }
    }
}
