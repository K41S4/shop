namespace CartApp.BusinessLogic.Exceptions
{
    /// <summary>
    /// Exception thrown when a quantity is negative.
    /// </summary>
    public class NegativeQuantityException() : Exception("Quantity cannot be negative");
}
