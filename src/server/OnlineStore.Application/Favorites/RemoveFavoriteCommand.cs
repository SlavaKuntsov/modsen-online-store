using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;

namespace OnlineStore.Application.Favorites;

public sealed record RemoveFavoriteCommand(Guid ProductId) : IRequest;

public sealed class RemoveFavoriteCommandHandler(IApplicationDbContext dbContext, IHttpContextAccessor accessor)
		: IRequestHandler<RemoveFavoriteCommand>
{
	public async Task Handle(RemoveFavoriteCommand request, CancellationToken ct)
	{
		var userIdStr = accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
		if (userIdStr is null || !Guid.TryParse(userIdStr, out var userId))
			throw new UnauthorizedAccessException("User not found");

		var favorite = await dbContext.Favorites
				.FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == request.ProductId, ct);
		if (favorite is not null)
		{
			dbContext.Favorites.Remove(favorite);
			await dbContext.SaveChangesAsync(ct);
		}
	}
}