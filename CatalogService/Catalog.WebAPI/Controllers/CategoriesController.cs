using AutoMapper;
using Catalog.Core.Entities;
using Catalog.Core.Services.Interfaces;
using Catalog.WebAPI.DTOs.Category;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebAPI.Controllers;

/// <summary>
/// Categories controller.
/// </summary>
/// <param name="categoryService">Category service.</param>
/// <param name="mapper">AutoMapper instance.</param>
[ApiController]
[Route("api/[controller]")]
public class CategoriesController(ICategoryService categoryService, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Endpoint for Category Create.
    /// </summary>
    /// <param name="dto">Category to create.</param>
    /// <returns>The response.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
    {
        var category = mapper.Map<Category>(dto);

        await categoryService.AddCategory(category);

        return this.Created();
    }

    /// <summary>
    /// Endpoint for Category Update.
    /// </summary>
    /// <param name="categoryId">Category id to update.</param>
    /// <param name="dto">New category values.</param>
    /// <returns>The response.</returns>
    [HttpPut("{categoryId}")]
    public async Task<IActionResult> UpdateCategory([FromRoute] int categoryId, [FromBody] UpdateCategoryDto dto)
    {
        var category = mapper.Map<Category>(dto);
        category.Id = categoryId;

        await categoryService.UpdateCategory(category);

        return this.NoContent();
    }

    /// <summary>
    /// Endpoint for Category Get by id.
    /// </summary>
    /// <param name="categoryId">Category id to get.</param>
    /// <returns>The response.</returns>
    [HttpGet("{categoryId}")]
    public async Task<IActionResult> GetCategory([FromRoute] int categoryId)
    {
        var category = await categoryService.GetCategory(categoryId);

        if (category is null)
        {
            return this.NotFound($"Category with {categoryId} id was not found.");
        }

        var categoryDto = mapper.Map<ResponseCategoryDto>(category);
        return this.Ok(categoryDto);
    }

    /// <summary>
    /// Endpoint for getting all Categories.
    /// </summary>
    /// <returns>The response.</returns>
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await categoryService.GetCategories();

        var categoriesDto = mapper.Map<IEnumerable<ResponseCategoryDto>>(categories);
        return this.Ok(categoriesDto);
    }

    /// <summary>
    /// Endpoint for Category remove.
    /// </summary>
    /// <param name="categoryId">Category to remove.</param>
    /// <returns>The response.</returns>
    [HttpDelete("{categoryId}")]
    public async Task<IActionResult> RemoveCategory([FromRoute] int categoryId)
    {
        await categoryService.RemoveCategory(categoryId);

        return this.NoContent();
    }
}
