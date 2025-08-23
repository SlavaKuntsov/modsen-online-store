using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.API.Contracts;
using OnlineStore.API.Contracts.PromoCode;
using OnlineStore.Application.Dtos;
using OnlineStore.Application.PromoCodes;

namespace OnlineStore.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/promocodes")]
[ApiVersion("1.0")]
[Authorize(Policy = "Admin")]
public class PromoCodeController(IMediator mediator) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> Get(CancellationToken ct = default)
	{
		var promos = await mediator.Send(new GetPromoCodesQuery(), ct);
		return Ok(new ApiResponse<List<PromoCodeDto>>(StatusCodes.Status200OK, promos, promos.Count));
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreatePromoCodeRequest request, CancellationToken ct = default)
	{
		var promo = await mediator.Send(new CreatePromoCodeCommand(request.Code, request.DiscountPercentage, request.ExpirationDate, request.IsActive), ct);
		return StatusCode(StatusCodes.Status201Created,
						new ApiResponse<PromoCodeDto>(StatusCodes.Status201Created, promo, 1));
	}

	[HttpPut("{id:guid}")]
	public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePromoCodeRequest request, CancellationToken ct = default)
	{
		var promo = await mediator.Send(new UpdatePromoCodeCommand(id, request.Code, request.DiscountPercentage, request.ExpirationDate, request.IsActive), ct);
		return Ok(new ApiResponse<PromoCodeDto>(StatusCodes.Status200OK, promo, 1));
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
	{
		await mediator.Send(new DeletePromoCodeCommand(id), ct);
		return NoContent();
	}
}