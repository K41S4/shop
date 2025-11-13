using System.Net;
using System.Net.Http.Json;
using Catalog.WebAPI;
using Catalog.WebAPI.DTOs.Product;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Catalog.Persistence.Entities;
using Shouldly;

namespace Catalog.IntegrationTests.WebApi.Controllers
{
    /// <summary>
    /// Integration tests for the ProductsController class.
    /// </summary>
    public class ProductsControllerIntegrationTests(CustomStartupWebApplicationFactory<FakeStartup, Startup> factory)
        : BaseIntegrationTest(factory)
    {
        /// <summary>
        /// Tests that CreateProduct creates a product successfully.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task CreateProduct_WithValidData_ShouldReturnOk()
        {
            // Arrange
            var categoryId = await this.SetupCategory();

            var createDto = new CreateProductDto
            {
                Name = "Test Product 123",
                Description = "Test Description",
                Price = 99.99m,
                Amount = 10,
                CategoryId = categoryId,
            };

            // Act
            var response = await this.HttpClient.PostAsJsonAsync("api/products", createDto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var product = await this.DbContext.Products.FirstOrDefaultAsync(p => p.Name == createDto.Name);
            product.Should().BeEquivalentTo(createDto);
        }

        /// <summary>
        /// Tests that GetProduct returns product when it exists.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task GetProduct_WithExistingId_ShouldReturnProduct()
        {
            // Arrange
            var categoryId = await this.SetupCategory();
            var product = await this.SetupProduct(categoryId);

            // Act
            var response = await this.HttpClient.GetAsync($"api/products/{product.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var productDto = await response.Content.ReadFromJsonAsync<ResponseProductDto>();
            productDto.Should().BeEquivalentTo(product);
        }

        /// <summary>
        /// Sets up a test category in the database.
        /// </summary>
        /// <returns>Id.</returns>
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

        /// <summary>
        /// Sets up a test product in the database.
        /// </summary>
        /// <returns>Product ID.</returns>
        private async Task<ProductEntity> SetupProduct(int categoryId)
        {
            var product = new ProductEntity
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 99.99m,
                Amount = 10,
                CategoryId = categoryId,
            };

            await this.DbContext.Products.AddAsync(product);
            await this.DbContext.SaveChangesAsync();
            return product;
        }
    }
}