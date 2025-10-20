namespace Catalog.Core.Entities.ValueObjects
{
    public class Price
    {
        public decimal Value { get; }

        private Price() {}

        public Price(decimal amount)
        {
            if (amount < 0)
            {
                throw new InvalidOperationException("Price cannot be negative");
            }

            this.Value = amount;
        }
    }
}
