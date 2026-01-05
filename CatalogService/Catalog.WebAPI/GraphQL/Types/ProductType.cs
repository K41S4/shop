using Catalog.Core.Entities;
using Catalog.WebAPI.GraphQL.DataLoaders;

namespace Catalog.WebAPI.GraphQL.Types;

/// <summary>
/// GraphQL type for Product.
/// </summary>
public class ProductType : ObjectType<Product>
{
    /// <inheritdoc/>
    protected override void Configure(IObjectTypeDescriptor<Product> descriptor)
    {
        descriptor.Name("Product");
        descriptor.Description("A product in the catalog.");

        descriptor
            .Field(p => p.Id)
            .Description("The unique identifier of the product.");

        descriptor
            .Field(p => p.Name)
            .Type<StringType>()
            .Resolve(context => context.Parent<Product>().Name.Value)
            .Description("The name of the product.");

        descriptor
            .Field(p => p.Description)
            .Type<StringType>()
            .Description("The description of the product.");

        descriptor
            .Field(p => p.Image)
            .Type<StringType>()
            .Resolve(context =>
            {
                var image = context.Parent<Product>().Image;
                return image?.Value;
            })
            .Description("The image URL of the product.");

        descriptor
            .Field(p => p.Price)
            .Type<DecimalType>()
            .Resolve(context => context.Parent<Product>().Price.Value)
            .Description("The price of the product.");

        descriptor
            .Field(p => p.Amount)
            .Type<IntType>()
            .Resolve(context => context.Parent<Product>().Amount.Value)
            .Description("The available amount of the product.");

        descriptor
            .Field(p => p.CategoryId)
            .Description("The category id this product belongs to.");

        descriptor
            .Field("category")
            .Type<CategoryType>()
            .Resolve(async context =>
            {
                var product = context.Parent<Product>();
                var dataLoader = context.DataLoader<CategoryDataLoader>();
                return await dataLoader.LoadAsync(product.CategoryId, context.RequestAborted);
            })
            .Description("The category this product belongs to.");
    }
}