namespace Catalog.Core.Entities.ValueObjects
{
    public class Amount
    {
        public int Value { get; }

        private Amount() {}

        public Amount(int amount)
        {
            if (amount < 0) throw new InvalidOperationException("Amount cannot be less than zero.");

            this.Value = amount;
        }
    }
}
