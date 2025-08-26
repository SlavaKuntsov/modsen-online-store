using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Minios.Services;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Application.ProductImages;

public sealed record AddProductImageCommand(Guid ProductId, IFormFile File) : IRequest<string>;

public sealed class AddProductImageCommandHandler(IApplicationDbContext dbContext, IMinioService minioService)
		: IRequestHandler<AddProductImageCommand, string>
{
	public async Task<string> Handle(AddProductImageCommand request, CancellationToken ct)
	{
                var product = await dbContext.Products
                        .Include(p => p.Image)
                        .FirstOrDefaultAsync(p => p.Id == request.ProductId, ct);
                if (product is null)
                        throw new NotFoundException($"Product with id '{request.ProductId}' not found");

                if (product.Image is not null)
                        throw new AlreadyExistsException("Product already has an image");

		var objectName = minioService.CreateObjectName(request.File.FileName);
		await minioService.UploadFileAsync(null, objectName, request.File.OpenReadStream(), request.File.ContentType);

                var image = new ProductImage
                {
                        Id = Guid.NewGuid(),
                        ProductId = product.Id,
                        ObjectName = objectName,
                        CreatedAt = DateTime.UtcNow
                };

                product.Image = image;
                await dbContext.SaveChangesAsync(ct);

		return await minioService.GetPresignedUrlAsync(null, objectName);
	}
}