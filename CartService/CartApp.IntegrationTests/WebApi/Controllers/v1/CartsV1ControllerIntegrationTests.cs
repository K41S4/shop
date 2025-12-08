using System.Net;
using System.Net.Http.Json;
using CartApp.IntegrationTests.WebApi.Common;
using CartApp.WebApi.Dtos;
using FluentAssertions;
using Mapster;
using Shouldly;

namespace CartApp.IntegrationTests.WebApi.Controllers.V1
{
    /// <summary>
    /// Integration tests for the CartsV1Controller class.
    /// </summary>
    [Collection("LiteDb collection")]
    public class CartsV1ControllerIntegrationTests(CustomStartupWebApplicationFactory<FakeStartup, Startup> factory, LiteDbFixture fixture)
        : BaseIntegrationTest(factory, fixture)
    {
        /// <summary>
        /// Tests that GetCartInfo returns cart information when cart exists.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task GetCartInfo_WithExistingCart_ShouldReturnCartInfo()
        {
            // Arrange
            const string cartId = "test-cart-1";
            var cartItemDto = new AddCartItem
            {
                Id = 1,
                Name = "Test Item 1",
                Price = 10.99m,
                Quantity = 2,
            };

            await this.HttpClient.PostAsJsonAsync($"api/v1/carts/{cartId}/items", cartItemDto);

            var cartItemDto2 = new AddCartItem
            {
                Id = 2,
                Name = "Test Item 2",
                Price = 5.50m,
                Quantity = 1,
            };

            await this.HttpClient.PostAsJsonAsync($"api/v1/carts/{cartId}/items", cartItemDto2);

            // Act
            var response = await this.HttpClient.GetAsync($"api/v1/carts/{cartId}");

            // Assert
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
            var cartDto = await response.Content.ReadFromJsonAsync<ResponseCart>();
            cartDto.Should().BeEquivalentTo(new ResponseCart
            {
                Id = cartId,
                Items = new List<ResponseCartItem>
                    { cartItemDto.Adapt<ResponseCartItem>(), cartItemDto2.Adapt<ResponseCartItem>() },
            });
        }

        /// <summary>
        /// Tests that AddItemToCart adds item to cart successfully.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task AddItemToCart_WithValidItem_ShouldAddItemToCart()
        {
            // Arrange
            const string cartId = "test-cart-2";
            var cartItemDto = new AddCartItem
            {
                Id = 1,
                Name = "New Item",
                Price = 15.99m,
                Quantity = 3,
            };

            // Act
            var response = await this.HttpClient.PostAsJsonAsync($"api/v1/carts/{cartId}/items", cartItemDto);

            // Assert
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

            var getResponse = await this.HttpClient.GetAsync($"api/v1/carts/{cartId}");
            getResponse.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
            var cartDto = await getResponse.Content.ReadFromJsonAsync<ResponseCart>();
            cartDto!.Items.Should().BeEquivalentTo(new List<AddCartItem> { cartItemDto }, options => options
                .ComparingByMembers<AddCartItem>());
        }
    }
}
