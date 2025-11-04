using System.Net;
using System.Net.Http.Json;
using CartApp.IntegrationTests.WebApi.Common;
using CartApp.WebApi.Dtos;
using FluentAssertions;

namespace CartApp.IntegrationTests.WebApi.Controllers.v2
{
    /// <summary>
    /// Integration tests for the CartsV2Controller class.
    /// </summary>
    [Collection("LiteDb collection")]
    public class CartsV2ControllerIntegrationTests(CustomStartupWebApplicationFactory<FakeStartup, Startup> factory, LiteDbFixture fixture)
        : BaseIntegrationTest(factory, fixture)
    {
        /// <summary>
        /// Tests that GetCartInfo returns cart items when cart exists.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task GetCartInfo_WithExistingCart_ShouldReturnCartItems()
        {
            // Arrange
            const string cartId = "test-cart-3";
            var cartItemDto = new AddCartItem
            {
                Id = 1,
                Name = "Test Item 1",
                Price = 10.99m,
                Quantity = 2,
            };

            await this.HttpClient.PostAsJsonAsync($"api/v2/carts/{cartId}/items", cartItemDto);

            var cartItemDto2 = new AddCartItem
            {
                Id = 2,
                Name = "Test Item 2",
                Price = 5.50m,
                Quantity = 1,
            };

            await this.HttpClient.PostAsJsonAsync($"api/v2/carts/{cartId}/items", cartItemDto2);

            // Act
            var response = await this.HttpClient.GetAsync($"api/v2/carts/{cartId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var cartItems = await response.Content.ReadFromJsonAsync<List<ResponseCartItem>>();
            cartItems.Should().NotBeNull();
            cartItems!.Count.Should().Be(2);
            cartItems[0].Name.Should().Be("Test Item 1");
            cartItems[1].Name.Should().Be("Test Item 2");
        }

        /// <summary>
        /// Tests that AddItemToCart adds item to cart successfully.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task AddItemToCart_WithValidItem_ShouldAddItemToCart()
        {
            // Arrange
            const string cartId = "test-cart-4";
            var cartItemDto = new AddCartItem
            {
                Id = 1,
                Name = "New Item",
                Price = 15.99m,
                Quantity = 3,
            };

            // Act
            var response = await this.HttpClient.PostAsJsonAsync($"api/v2/carts/{cartId}/items", cartItemDto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var getResponse = await this.HttpClient.GetAsync($"api/v2/carts/{cartId}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var cartItems = await getResponse.Content.ReadFromJsonAsync<List<ResponseCartItem>>();
            cartItems.Should().NotBeNull();
            cartItems!.Should().Contain(item =>
                item.Id == cartItemDto.Id &&
                item.Name == cartItemDto.Name &&
                item.Price == cartItemDto.Price &&
                item.Quantity == cartItemDto.Quantity);
        }
    }
}
