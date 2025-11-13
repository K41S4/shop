namespace CartApp.BusinessLogic.Exceptions
{
    /// <summary>
    /// Exception thrown when a requested resource is not found.
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
