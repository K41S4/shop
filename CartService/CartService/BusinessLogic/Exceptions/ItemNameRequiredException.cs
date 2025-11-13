namespace CartApp.BusinessLogic.Exceptions
{
    /// <summary>
    /// Exception thrown when an item name is required but not provided.
    /// </summary>
    public class ItemNameRequiredException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemNameRequiredException"/> class.
        /// </summary>
        public ItemNameRequiredException()
            : base("Item name is required")
        {
        }
    }
}
