using System.Net;
using System.Net.Http.Json;
using Catalog.Persistence.Entities;
using Catalog.WebAPI;
using Catalog.WebAPI.DTOs.Category;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace Catalog.IntegrationTests.WebApi.Controllers
{
    /// <summary>
    /// Integration tests for the CategoriesController class.
    /// </summary>
    public class CategoriesControllerIntegrationTests(CustomStartupWebApplicationFactory<FakeStartup, Startup> factory)
        : BaseIntegrationTest(factory)
    {
        /// <summary>
        /// Tests that CreateCategory creates a category successfully.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task CreateCategory_WithValidData_ShouldReturnCreated()
        {
            // Arrange
            var createDto = new CreateCategoryDto
            {
                Name = "Test",
            };

            // Act
            var response = await this.HttpClient.PostAsJsonAsync("api/Categories", createDto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var category = await this.DbContext.Categories.FirstOrDefaultAsync(c => c.Name == "Test");
            category?.Name.ShouldBe("Test");
        }

        /// <summary>
        /// Tests that GetCategory returns category when it exists.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task GetCategory_WithExistingId_ShouldReturnCategory()
        {
            // Arrange
            var categoryId = await this.SetupCategory();

            // Act
            var response = await this.HttpClient.GetAsync($"api/categories/{categoryId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var categoryDto = await response.Content.ReadFromJsonAsync<ResponseCategoryDto>();
            categoryDto.ShouldBeEquivalentTo(new ResponseCategoryDto { Id = categoryId, Name = "Test Category" });
        }

        /// <summary>
        /// Sets up a test category in the database.
        /// </summary>
        /// <returns>Category ID.</returns>
        private async Task<int> SetupCategory()
        {
            var category = new CategoryEntity
            {
                Name = "Test Category",
            };

            await this.DbContext.Categories.AddAsync(category);
            await this.DbContext.SaveChangesAsync();
            return category.Id;
        }
    }
}