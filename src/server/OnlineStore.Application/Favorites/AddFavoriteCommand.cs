using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;

namespace OnlineStore.Application.Favorites;

public sealed record AddFavoriteCommand(Guid ProductId) : IRequest;

public sealed class AddFavoriteCommandHandler(IApplicationDbContext dbContext, IHttpContextAccessor accessor)
		: IRequestHandler<AddFavoriteCommand>
{
	public async Task Handle(AddFavoriteCommand request, CancellationToken ct)
	{
		var userIdStr = accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
		if (userIdStr is null || !Guid.TryParse(userIdStr, out var userId))
			throw new UnauthorizedAccessException("User not found");

		var product = await dbContext.Products.FindAsync([request.ProductId], ct);
		if (product is null)
			throw new InvalidOperationException("Product not found");

		var exists = await dbContext.Favorites.AnyAsync(f => f.UserId == userId && f.ProductId == request.ProductId, ct);
		if (!exists)
		{
			await dbContext.Favorites.AddAsync(new(userId, request.ProductId), ct);
			await dbContext.SaveChangesAsync(ct);
		}
	}
}