namespace Catalog.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a price is negative.
    /// </summary>
    public class NegativePriceException() : Exception("Price cannot be negative.");
}
