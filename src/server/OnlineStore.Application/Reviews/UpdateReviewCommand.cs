using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Application.Dtos;

namespace OnlineStore.Application.Reviews;

public sealed record UpdateReviewCommand(Guid Id, int Rating, string Comment) : IRequest<ReviewDto>;

public sealed class UpdateReviewCommandHandler(IApplicationDbContext dbContext)
        : IRequestHandler<UpdateReviewCommand, ReviewDto>
{
        public async Task<ReviewDto> Handle(UpdateReviewCommand request, CancellationToken ct)
        {
                var review = await dbContext.ProductReviews.FirstOrDefaultAsync(r => r.Id == request.Id, ct)
                        ?? throw new InvalidOperationException("Review not found");

                review.Rating = request.Rating;
                review.Comment = request.Comment;
                await dbContext.SaveChangesAsync(ct);

                var productReviews = await dbContext.ProductReviews
                        .Where(r => r.ProductId == review.ProductId)
                        .ToListAsync(ct);
                var product = await dbContext.Products.FindAsync([review.ProductId], ct)
                        ?? throw new InvalidOperationException("Product not found");
                product.Rating = productReviews.Count > 0 ? productReviews.Average(r => r.Rating) : 0;
                await dbContext.SaveChangesAsync(ct);

                return new(review.Id, review.ProductId, review.UserId, review.Rating, review.Comment, review.CreatedAt);
        }
}
