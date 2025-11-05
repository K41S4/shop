using AutoMapper;
using Catalog.Core.Entities;
using Catalog.Core.Entities.ValueObjects;
using Catalog.Core.Services.Interfaces;
using Catalog.WebAPI.Controllers;
using Catalog.WebAPI.DTOs.Category;
using Catalog.WebAPI.Mapping;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Shouldly;

namespace Catalog.WebAPI.UnitTests.Controllers
{
    /// <summary>
    /// Unit tests for the CategoriesController class.
    /// </summary>
    public class CategoriesControllerTests
    {
        private readonly IMapper mapper;
        private readonly Mock<ICategoryService> mockService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesControllerTests"/> class.
        /// </summary>
        public CategoriesControllerTests()
        {
            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.AddProfile<CategoryDtosProfile>();
                },
                new NullLoggerFactory());
            this.mapper = config.CreateMapper();

            this.mockService = new Mock<ICategoryService>();
        }

        /// <summary>
        /// Tests that CreateCategory returns Created when category is successfully created.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task CreateCategory_WithValidData_ShouldReturnCreated()
        {
            // Arrange
            var createDto = new CreateCategoryDto
            {
                Name = "Test Category",
                Image = "https://example.com/image.jpg",
            };

            this.mockService.Setup(s => s.AddCategory(It.IsAny<Category>()))
                .Returns(Task.CompletedTask);

            var controller = new CategoriesController(this.mockService.Object, this.mapper);

            // Act
            var result = await controller.CreateCategory(createDto);

            // Assert
            result.Should().BeOfType<CreatedResult>();
            this.mockService.Verify(s => s.AddCategory(It.IsAny<Category>()), Times.Once);
        }

        /// <summary>
        /// Tests that GetCategory returns Ok with category DTO when category exists.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task GetCategory_WithExistingId_ShouldReturnOk()
        {
            // Arrange
            const int categoryId = 1;
            var category = new Category
            {
                Id = categoryId,
                Name = new Name { Value = "Test Category" },
                Image = new Url { Value = "https://example.com/image.jpg" },
            };

            this.mockService.Setup(s => s.GetCategory(categoryId))
                .ReturnsAsync(category);

            var controller = new CategoriesController(this.mockService.Object, this.mapper);

            // Act
            var result = await controller.GetCategory(categoryId);

            // Assert
            result.ShouldBeEquivalentTo(new OkObjectResult(new ResponseCategoryDto
            {
                Id = categoryId,
                Name = "Test Category",
                Image = "https://example.com/image.jpg",
            }));
        }
    }
}