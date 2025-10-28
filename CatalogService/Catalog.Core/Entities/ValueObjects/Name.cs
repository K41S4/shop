using Catalog.Core.Exceptions;

namespace Catalog.Core.Entities.ValueObjects
{
    /// <summary>
    /// Represents a name value object with validation.
    /// </summary>
    public class Name
    {
        private readonly string? nameValue;

        /// <summary>
        /// Gets the name value. Must not be null, empty, or exceed 50 characters.
        /// </summary>
        public required string? Value
        {
            get => this.nameValue;
            init
            {
                if (string.IsNullOrEmpty(value) || value.Length > 50)
                {
                    throw new InvalidNameException();
                }

                this.nameValue = value;
            }
        }
    }
}
