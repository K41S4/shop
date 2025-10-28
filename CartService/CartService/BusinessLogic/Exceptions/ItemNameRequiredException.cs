namespace CartApp.BusinessLogic.Exceptions
{
    /// <summary>
    /// Exception thrown when an item name is required but not provided.
    /// </summary>
    public class ItemNameRequiredException() : Exception("Item name is required");
}
