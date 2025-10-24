using Catalog.Core.Exceptions;

namespace Catalog.Core.Entities.ValueObjects
{
    /// <summary>
    /// Represents a URL value object with validation.
    /// </summary>
    public class Url
    {
        private readonly string? urlValue;

        /// <summary>
        /// Gets the URL value. Must be a valid absolute URI if not null or empty.
        /// </summary>
        public required string? Value
        {
            get => this.urlValue;
            init
            {
                if (!string.IsNullOrEmpty(value) && !Uri.IsWellFormedUriString(value, UriKind.Absolute))
                {
                    throw new InvalidUrlException();
                }

                this.urlValue = value;
            }
        }
    }
}
