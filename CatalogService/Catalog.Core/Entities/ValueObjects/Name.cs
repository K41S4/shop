using InvalidOperationException = System.InvalidOperationException;

namespace Catalog.Core.Entities.ValueObjects
{
    public class Name
    {
        public string Value { get; }

        private Name() {}

        public Name(string name)
        {
            if (name.Length > 50)
            {
                throw new InvalidOperationException("Name must be less than 50 characters.");
            }

            this.Value = name;
        }
    }
}
