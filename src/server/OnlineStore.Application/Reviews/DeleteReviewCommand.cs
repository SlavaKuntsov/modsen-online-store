using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;

namespace OnlineStore.Application.Reviews;

public sealed record DeleteReviewCommand(Guid Id) : IRequest;

public sealed class DeleteReviewCommandHandler(IApplicationDbContext dbContext)
        : IRequestHandler<DeleteReviewCommand>
{
        public async Task Handle(DeleteReviewCommand request, CancellationToken ct)
        {
                var review = await dbContext.ProductReviews.FirstOrDefaultAsync(r => r.Id == request.Id, ct)
                        ?? throw new InvalidOperationException("Review not found");

                dbContext.ProductReviews.Remove(review);
                await dbContext.SaveChangesAsync(ct);

                var productReviews = await dbContext.ProductReviews
                        .Where(r => r.ProductId == review.ProductId)
                        .ToListAsync(ct);
                var product = await dbContext.Products.FindAsync([review.ProductId], ct)
                        ?? throw new InvalidOperationException("Product not found");
                product.Rating = productReviews.Count > 0 ? productReviews.Average(r => r.Rating) : 0;
                await dbContext.SaveChangesAsync(ct);
        }
}
