using System.Security.Claims;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.API.Contracts;
using OnlineStore.API.Contracts.Order;
using OnlineStore.Application.Dtos;
using OnlineStore.Application.Orders;
using OnlineStore.Domain.Enums;

namespace OnlineStore.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/orders")]
[ApiVersion("1.0")]
public class OrderController(IMediator mediator) : ControllerBase
{
	[HttpGet]
	[Authorize(Policy = "User")]
	public async Task<IActionResult> Get(CancellationToken ct = default)
	{
		var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
				?? throw new UnauthorizedAccessException("User ID not found in claims.");
		if (!Guid.TryParse(userIdClaim.Value, out var userId))
			throw new UnauthorizedAccessException("Invalid User ID format in claims.");

		var orders = await mediator.Send(new GetOrdersQuery(userId), ct);
		return Ok(new ApiResponse<List<OrderDto>>(StatusCodes.Status200OK, orders, orders.Count));
	}

	[HttpGet("{id:guid}")]
	[Authorize(Policy = "User")]
	public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
	{
		var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
				?? throw new UnauthorizedAccessException("User ID not found in claims.");
		if (!Guid.TryParse(userIdClaim.Value, out var userId))
			throw new UnauthorizedAccessException("Invalid User ID format in claims.");

		var order = await mediator.Send(new GetOrderByIdQuery(id, userId), ct);
		return Ok(new ApiResponse<OrderDto>(StatusCodes.Status200OK, order, 1));
	}

        [HttpPost]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> Create([FromBody] PlaceOrderRequest request, CancellationToken ct = default)
        {
                var order = await mediator.Send(new PlaceOrderCommand(request.ShippingAddress, request.DeliveryMethod, request.PromoCode), ct);
                return StatusCode(StatusCodes.Status201Created,
                new ApiResponse<OrderDto>(StatusCodes.Status201Created, order, 1));
        }

        [HttpPost("pay")]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> Pay([FromBody] PayOrderRequest request, CancellationToken ct = default)
        {
                var order = await mediator.Send(new PayOrderCommand(request.OrderId), ct);
                return Ok(new ApiResponse<OrderDto>(StatusCodes.Status200OK, order, 1));
        }

	[HttpGet("delivery-methods")]
	public IActionResult GetDeliveryMethods()
	{
		var methods = Enum.GetValues<DeliveryMethod>()
				.Select(m => new DeliveryMethodDto((int)m, m.ToString()))
				.ToList();
		return Ok(new ApiResponse<List<DeliveryMethodDto>>(StatusCodes.Status200OK, methods, methods.Count));
	}
}