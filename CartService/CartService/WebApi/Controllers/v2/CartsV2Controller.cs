using Asp.Versioning;
using CartApp.BusinessLogic.Services;
using CartApp.Models;
using CartApp.WebApi.Dtos;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CartApp.WebApi.Controllers.V2;

/// <summary>
/// Controller for Cart operations.
/// </summary>
/// <param name="service">Cart service.</param>
/// <param name="mapper">Mapper.</param>
[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/carts")]
[Authorize(Roles = "Manager,StoreCustomer")]
public class CartsV2Controller(ICartService service, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Endpoint to Get Cart Items by cart id.
    /// </summary>
    /// <param name="cartId">Cart id to get items from.</param>
    /// <returns>The response.</returns>
    [HttpGet("{cartId}")]
    public async Task<IActionResult> GetCartInfo([FromRoute] string cartId)
    {
        var cartItems = await service.GetCartItems(cartId);

        var cartItemsDro = mapper.Map<List<ResponseCartItem>>(cartItems);

        return this.Ok(cartItemsDro);
    }

    /// <summary>
    /// Endpoint to Adding Item to Cart.
    /// </summary>
    /// <param name="cartId">Cart id to add the item to.</param>
    /// <param name="cartItemDto">Cart item to add to cart.</param>
    /// <returns>The response.</returns>
    [HttpPost("{cartId}/items")]
    public async Task<IActionResult> AddItemToCart([FromRoute] string cartId, [FromBody] AddCartItem cartItemDto)
    {
        var cartItem = mapper.Map<CartItem>(cartItemDto);
        await service.AddItemToCart(cartId, cartItem);
        return this.Ok();
    }

    /// <summary>
    /// Endpoint to Delete Item from Cart.
    /// </summary>
    /// <param name="cartId">Cart id to delete item from.</param>
    /// <param name="itemId">Item id to delete.</param>
    /// <returns>The response.</returns>
    [HttpDelete("{cartId}/items/{itemId}")]
    public async Task<IActionResult> DeleteItemFromCart([FromRoute] string cartId, [FromRoute] int itemId)
    {
        await service.RemoveItemFromCart(cartId, itemId);

        return this.Ok();
    }
}
