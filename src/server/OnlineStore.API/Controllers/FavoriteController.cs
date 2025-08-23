using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.API.Contracts;
using OnlineStore.Application.Dtos;
using OnlineStore.Application.Favorites;

namespace OnlineStore.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/favorites")]
[ApiVersion("1.0")]
[Authorize(Policy = "User")]
public class FavoriteController(IMediator mediator) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> Get(CancellationToken ct = default)
	{
		var favorites = await mediator.Send(new GetFavoritesQuery(), ct);
		return Ok(new ApiResponse<List<FavoriteDto>>(StatusCodes.Status200OK, favorites, favorites.Count));
	}

	[HttpPost("{productId:guid}")]
	public async Task<IActionResult> Add(Guid productId, CancellationToken ct = default)
	{
		await mediator.Send(new AddFavoriteCommand(productId), ct);
		return Ok(new ApiResponse<string>(StatusCodes.Status200OK, "Added", 0));
	}

	[HttpDelete("{productId:guid}")]
	public async Task<IActionResult> Remove(Guid productId, CancellationToken ct = default)
	{
		await mediator.Send(new RemoveFavoriteCommand(productId), ct);
		return Ok(new ApiResponse<string>(StatusCodes.Status200OK, "Removed", 0));
	}
}