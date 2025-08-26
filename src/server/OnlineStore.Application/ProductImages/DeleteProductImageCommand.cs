using MediatR;
using Microsoft.EntityFrameworkCore;
using Minios.Services;
using OnlineStore.Application.Abstractions.Data;

namespace OnlineStore.Application.ProductImages;

public sealed record DeleteProductImageCommand(Guid ImageId) : IRequest;

public sealed class DeleteProductImageCommandHandler(IApplicationDbContext dbContext, IMinioService minioService)
        : IRequestHandler<DeleteProductImageCommand>
{
    public async Task Handle(DeleteProductImageCommand request, CancellationToken ct)
    {
        var image = await dbContext.ProductImages
            .FirstOrDefaultAsync(i => i.Id == request.ImageId, ct);
        if (image is null)
            return;

        await minioService.RemoveFileAsync(null, image.ObjectName);
        dbContext.ProductImages.Remove(image);
        await dbContext.SaveChangesAsync(ct);
    }
}
