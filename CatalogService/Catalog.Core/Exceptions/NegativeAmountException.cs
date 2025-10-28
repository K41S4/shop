namespace Catalog.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when an amount is negative.
    /// </summary>
    public class NegativeAmountException() : Exception("Amount cannot be less than zero.");
}
