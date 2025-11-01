using AutoMapper;
using Catalog.Core.Entities;
using Catalog.Core.Services.Interfaces;
using Catalog.WebAPI.DTOs.Product;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebAPI.Controllers;

/// <summary>
/// Products controller.
/// </summary>
/// <param name="productService">Product service.</param>
[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Endpoint for Product Create.
    /// </summary>
    /// <param name="dto">Product to create.</param>
    /// <returns>The response.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto)
    {
        var product = mapper.Map<Product>(dto);

        await productService.AddProduct(product);

        return this.Ok();
    }

    /// <summary>
    /// Endpoint for Product Update.
    /// </summary>
    /// <param name="productId">Product id to update.</param>
    /// <param name="dto">New product values.</param>
    /// <returns>The response.</returns>
    [HttpPut("{productId}")]
    public async Task<IActionResult> UpdateProduct([FromRoute] int productId, [FromBody] UpdateProductDto dto)
    {
        var product = mapper.Map<Product>(dto);
        product.Id = productId;

        await productService.UpdateProduct(product);

        return this.NoContent();
    }

    /// <summary>
    /// Endpoint for Product Get by id.
    /// </summary>
    /// <param name="productId">Product id to get.</param>
    /// <returns>The response.</returns>
    [HttpGet("{productId}")]
    public async Task<IActionResult> GetProduct([FromRoute] int productId)
    {
        var product = await productService.GetProduct(productId);

        var productDto = mapper.Map<ResponseProductDto>(product);
        return this.Ok(productDto);
    }

    /// <summary>
    /// Endpoint for getting all products.
    /// </summary>
    /// <returns>The response.</returns>
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await productService.GetProducts();

        var productsDto = mapper.Map<IEnumerable<ResponseProductDto>>(products);
        return this.Ok(productsDto);
    }

    /// <summary>
    /// Endpoint for Product remove.
    /// </summary>
    /// <param name="productId">Product to remove.</param>
    /// <returns>The response.</returns>
    [HttpDelete("{productId}")]
    public async Task<IActionResult> RemoveProduct([FromRoute] int productId)
    {
        var removed = await productService.RemoveProduct(productId);
        if (!removed)
        {
            return this.NotFound();
        }

        return this.NoContent();
    }
}
