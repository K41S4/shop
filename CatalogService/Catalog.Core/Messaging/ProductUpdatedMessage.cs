namespace Catalog.Core.Messaging;

/// <summary>
/// Represents a message sent when a product is updated.
/// </summary>
public class ProductUpdatedMessage
{
    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    public required int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the product price.
    /// </summary>
    public required decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the product image URL.
    /// </summary>
    public string? ImageUrl { get; set; }
}