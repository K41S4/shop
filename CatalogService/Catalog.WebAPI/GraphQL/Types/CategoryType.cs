using Catalog.Core.Entities;
using Catalog.WebAPI.GraphQL.DataLoaders;

namespace Catalog.WebAPI.GraphQL.Types;

/// <summary>
/// GraphQL type for Category.
/// </summary>
public class CategoryType : ObjectType<Category>
{
    /// <inheritdoc/>
    protected override void Configure(IObjectTypeDescriptor<Category> descriptor)
    {
        descriptor.Name("Category");
        descriptor.Description("A category in the catalog.");

        descriptor
            .Field(c => c.Id)
            .Description("The unique identifier of the category.");

        descriptor
            .Field(c => c.Name)
            .Type<StringType>()
            .Resolve(context => context.Parent<Category>().Name.Value)
            .Description("The name of the category.");

        descriptor
            .Field(c => c.Image)
            .Type<StringType>()
            .Resolve(context =>
            {
                var image = context.Parent<Category>().Image;
                return image?.Value;
            })
            .Description("The image URL of the category.");

        descriptor
            .Field(c => c.ParentCategoryId)
            .Type<IntType>()
            .Description("The parent category id. Null for root categories.");

        descriptor
            .Field("products")
            .Type<ListType<ProductType>>()
            .Resolve(async context =>
            {
                var category = context.Parent<Category>();
                var dataLoader = context.DataLoader<ProductsByCategoryDataLoader>();
                return await dataLoader.LoadAsync(category.Id, context.RequestAborted);
            })
            .Description("The products in this category.");
    }
}