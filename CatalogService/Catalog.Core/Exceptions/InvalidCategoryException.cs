namespace Catalog.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a category does not exist.
    /// </summary>
    public class InvalidCategoryException() : Exception("Category does not exist.");
}
