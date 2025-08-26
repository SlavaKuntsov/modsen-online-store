using MediatR;
using Microsoft.EntityFrameworkCore;
using Minios.Services;
using OnlineStore.Application.Abstractions.Data;

namespace OnlineStore.Application.ProductImages;

public sealed record DeleteProductImageCommand(Guid ProductId) : IRequest;

public sealed class DeleteProductImageCommandHandler(IApplicationDbContext dbContext, IMinioService minioService)
		: IRequestHandler<DeleteProductImageCommand>
{
	public async Task Handle(DeleteProductImageCommand request, CancellationToken ct)
	{
                var product = await dbContext.Products
                        .Include(p => p.Image)
                        .FirstOrDefaultAsync(p => p.Id == request.ProductId, ct);
                if (product?.Image is null)
                        return;

                await minioService.RemoveFileAsync(null, product.Image.ObjectName);
                dbContext.ProductImages.Remove(product.Image);
                await dbContext.SaveChangesAsync(ct);
        }
}