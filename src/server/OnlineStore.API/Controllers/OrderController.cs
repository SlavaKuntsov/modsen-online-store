using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.API.Contracts;
using OnlineStore.Application.Dtos;
using OnlineStore.Application.Orders;

namespace OnlineStore.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/orders")]
[ApiVersion("1.0")]
public class OrderController(IMediator mediator) : ControllerBase
{
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlaceOrderRequest request, CancellationToken ct = default)
        {
                var order = await mediator.Send(new PlaceOrderCommand(request.ShippingAddress, request.DeliveryMethod), ct);
                return StatusCode(StatusCodes.Status201Created,
                        new ApiResponse<OrderDto>(StatusCodes.Status201Created, order, 1));
        }

        [HttpPost("pay")]
        public async Task<IActionResult> Pay([FromBody] PayOrderRequest request, CancellationToken ct = default)
        {
                var order = await mediator.Send(new PayOrderCommand(request.OrderId), ct);
                return Ok(new ApiResponse<OrderDto>(StatusCodes.Status200OK, order, 1));
        }
}
