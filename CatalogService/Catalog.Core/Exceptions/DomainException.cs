namespace Catalog.Core.Exceptions
{
    /// <summary>
    /// Domain exception.
    /// </summary>
    public abstract class DomainException(string message) : Exception(message);
}
