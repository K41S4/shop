using Catalog.Core.Entities;
using Catalog.Core.Entities.ValueObjects;
using Catalog.Core.Services.Interfaces;
using HotChocolate.Authorization;

namespace Catalog.WebAPI.GraphQL.Mutations;

/// <summary>
/// GraphQLs mutations for catalog operations.
/// </summary>
public class CatalogMutations
{
    /// <summary>
    /// Adds a new category.
    /// </summary>
    /// <param name="categoryService">The category service.</param>
    /// <param name="name">The name of the category.</param>
    /// <param name="image">The image URL of the category.</param>
    /// <param name="parentCategoryId">The parent category id. Null for root categories.</param>
    /// <returns>The created category.</returns>
    [Authorize(Policy = "Create")]
    [GraphQLDescription("Adds a new category.")]
    public async Task<Category> AddCategory(
        [Service] ICategoryService categoryService,
        string name,
        string? image = null,
        int? parentCategoryId = null)
    {
        var category = new Category
        {
            Name = new Name { Value = name },
            Image = image != null ? new Url { Value = image } : null,
            ParentCategoryId = parentCategoryId,
        };

        await categoryService.AddCategory(category);
        return category;
    }

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    /// <param name="categoryService">The category service.</param>
    /// <param name="id">The category id to update.</param>
    /// <param name="name">The new name of the category.</param>
    /// <param name="image">The new image URL of the category.</param>
    /// <param name="parentCategoryId">The new parent category id.</param>
    /// <returns>The updated category.</returns>
    [Authorize(Policy = "Update")]
    [GraphQLDescription("Updates an existing category.")]
    public async Task<Category> UpdateCategory(
        [Service] ICategoryService categoryService,
        int id,
        string name,
        string? image = null,
        int? parentCategoryId = null)
    {
        var category = new Category
        {
            Id = id,
            Name = new Name { Value = name },
            Image = image != null ? new Url { Value = image } : null,
            ParentCategoryId = parentCategoryId,
        };

        await categoryService.UpdateCategory(category);
        var updatedCategory = await categoryService.GetCategory(id);

        return updatedCategory!;
    }

    /// <summary>
    /// Deletes a category and its related products.
    /// </summary>
    /// <param name="categoryService">The category service.</param>
    /// <param name="productService">The product service.</param>
    /// <param name="id">The category id to delete.</param>
    /// <returns>The id of the deleted category.</returns>
    [Authorize(Policy = "Delete")]
    [GraphQLDescription("Deletes a category and its related products.")]
    public async Task<int> DeleteCategory(
        [Service] ICategoryService categoryService,
        [Service] IProductService productService,
        int id)
    {
        await productService.RemoveProductsByCategoryId(id);
        await categoryService.RemoveCategory(id);

        return id;
    }

    /// <summary>
    /// Adds a new product.
    /// </summary>
    /// <param name="productService">The product service.</param>
    /// <param name="name">The name of the product.</param>
    /// <param name="description">The description of the product.</param>
    /// <param name="image">The image URL of the product.</param>
    /// <param name="price">The price of the product.</param>
    /// <param name="amount">The available amount of the product.</param>
    /// <param name="categoryId">The category id this product belongs to.</param>
    /// <returns>The created product.</returns>
    [Authorize(Policy = "Create")]
    [GraphQLDescription("Adds a new product.")]
    public async Task<Product> AddProduct(
        [Service] IProductService productService,
        string name,
        string? description = null,
        string? image = null,
        decimal price = 0,
        int amount = 0,
        int categoryId = 0)
    {
        var product = new Product
        {
            Name = new Name { Value = name },
            Description = description,
            Image = image != null ? new Url { Value = image } : null,
            Price = new Price { Value = price },
            Amount = new Amount { Value = amount },
            CategoryId = categoryId,
        };

        await productService.AddProduct(product);
        return product;
    }

    /// <summary>
    /// Updates an existing product.
    /// </summary>
    /// <param name="productService">The product service.</param>
    /// <param name="id">The product id to update.</param>
    /// <param name="name">The new name of the product.</param>
    /// <param name="description">The new description of the product.</param>
    /// <param name="image">The new image URL of the product.</param>
    /// <param name="price">The new price of the product.</param>
    /// <param name="amount">The new available amount of the product.</param>
    /// <param name="categoryId">The new category id this product belongs to.</param>
    /// <returns>The updated product.</returns>
    [Authorize(Policy = "Update")]
    [GraphQLDescription("Updates an existing product.")]
    public async Task<Product> UpdateProduct(
        [Service] IProductService productService,
        int id,
        string name,
        string? description = null,
        string? image = null,
        decimal price = 0,
        int amount = 0,
        int categoryId = 0)
    {
        var product = new Product
        {
            Id = id,
            Name = new Name { Value = name },
            Description = description,
            Image = image != null ? new Url { Value = image } : null,
            Price = new Price { Value = price },
            Amount = new Amount { Value = amount },
            CategoryId = categoryId,
        };

        await productService.UpdateProduct(product);
        var updatedProduct = await productService.GetProduct(id);
        return updatedProduct!;
    }

    /// <summary>
    /// Deletes a product.
    /// </summary>
    /// <param name="productService">The product service.</param>
    /// <param name="id">The product id to delete.</param>
    /// <returns>The id of the deleted product.</returns>
    [Authorize(Policy = "Delete")]
    [GraphQLDescription("Deletes a product.")]
    public async Task<int> DeleteProduct(
        [Service] IProductService productService,
        int id)
    {
        await productService.RemoveProduct(id);
        return id;
    }
}