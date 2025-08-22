using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Application.Dtos;

namespace OnlineStore.Application.Reviews;

public sealed record GetReviewsQuery(Guid ProductId) : IRequest<List<ReviewDto>>;

public sealed class GetReviewsQueryHandler(IApplicationDbContext dbContext)
        : IRequestHandler<GetReviewsQuery, List<ReviewDto>>
{
        public async Task<List<ReviewDto>> Handle(GetReviewsQuery request, CancellationToken ct)
        {
                var reviews = await dbContext.ProductReviews
                        .Where(r => r.ProductId == request.ProductId)
                        .OrderByDescending(r => r.CreatedAt)
                        .ToListAsync(ct);

                return reviews.Select(r => new ReviewDto(r.Id, r.ProductId, r.UserId, r.Rating, r.Comment, r.CreatedAt)).ToList();
        }
}
