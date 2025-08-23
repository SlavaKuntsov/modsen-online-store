using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Application.Dtos;

namespace OnlineStore.Application.Favorites;

public sealed record GetFavoritesQuery() : IRequest<List<FavoriteDto>>;

public sealed class GetFavoritesQueryHandler(IApplicationDbContext dbContext, IHttpContextAccessor accessor)
		: IRequestHandler<GetFavoritesQuery, List<FavoriteDto>>
{
	public async Task<List<FavoriteDto>> Handle(GetFavoritesQuery request, CancellationToken ct)
	{
		var userIdStr = accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
		if (userIdStr is null || !Guid.TryParse(userIdStr, out var userId))
			throw new UnauthorizedAccessException("User not found");

		var favorites = await dbContext.Favorites
				.Where(f => f.UserId == userId)
				.Include(f => f.Product)
				.ToListAsync(ct);

		return favorites.Select(f => new FavoriteDto(f.ProductId, f.Product.Name, f.Product.Price)).ToList();
	}
}