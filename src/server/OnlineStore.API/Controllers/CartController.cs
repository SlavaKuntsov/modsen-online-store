using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.API.Contracts;
using OnlineStore.API.Services;
using OnlineStore.Domain.Entities;

namespace OnlineStore.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/cart")]
[ApiVersion("1.0")]
public class CartController(ICartService cartService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var cart = await cartService.GetCartAsync();
        return Ok(new ApiResponse<CartDto>(StatusCodes.Status200OK, Map(cart), cart.Items.Count));
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddItem([FromBody] AddCartItemRequest request)
    {
        var cart = await cartService.AddItemAsync(request.ProductId, request.Quantity);
        return Ok(new ApiResponse<CartDto>(StatusCodes.Status200OK, Map(cart), cart.Items.Count));
    }

    [HttpPut("items")]
    public async Task<IActionResult> UpdateItem([FromBody] UpdateCartItemRequest request)
    {
        var cart = await cartService.UpdateItemQuantityAsync(request.ProductId, request.Quantity);
        return Ok(new ApiResponse<CartDto>(StatusCodes.Status200OK, Map(cart), cart.Items.Count));
    }

    [HttpDelete("items/{productId}")]
    public async Task<IActionResult> RemoveItem(Guid productId)
    {
        var cart = await cartService.RemoveItemAsync(productId);
        return Ok(new ApiResponse<CartDto>(StatusCodes.Status200OK, Map(cart), cart.Items.Count));
    }

    private static CartDto Map(Cart cart)
    {
        var items = cart.Items.Select(i => new CartItemDto(i.ProductId, i.ProductName, i.UnitPrice, i.Quantity, i.UnitPrice * i.Quantity)).ToList();
        return new CartDto(items, items.Sum(i => i.SubTotal));
    }
}
