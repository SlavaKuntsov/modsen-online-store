using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Application.Dtos;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Application.Reviews;

public sealed record CreateReviewCommand(Guid ProductId, int Rating, string Comment) : IRequest<ReviewDto>;

public sealed class CreateReviewCommandHandler(IApplicationDbContext dbContext, IHttpContextAccessor accessor)
		: IRequestHandler<CreateReviewCommand, ReviewDto>
{
	public async Task<ReviewDto> Handle(CreateReviewCommand request, CancellationToken ct)
	{
		var userIdStr = accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
		if (userIdStr is null || !Guid.TryParse(userIdStr, out var userId))
			throw new UnauthorizedAccessException("User not found");

		if (request.Rating < 1 || request.Rating > 5)
			throw new ArgumentOutOfRangeException(nameof(request.Rating));

		var product = await dbContext.Products.FindAsync([request.ProductId], ct);
		if (product is null)
			throw new InvalidOperationException("Product not found");

		ProductReview review = new(request.ProductId, userId, request.Rating, request.Comment);
		await dbContext.ProductReviews.AddAsync(review, ct);
		await dbContext.SaveChangesAsync(ct);

		product.Rating = await dbContext.ProductReviews
				.Where(r => r.ProductId == request.ProductId)
				.AverageAsync(r => r.Rating, ct);
		await dbContext.SaveChangesAsync(ct);

		return new(review.Id, review.ProductId, review.UserId, review.Rating, review.Comment, review.CreatedAt);
	}
}