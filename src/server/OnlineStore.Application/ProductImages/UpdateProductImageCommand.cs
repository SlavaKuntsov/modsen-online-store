using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Minios.Services;
using OnlineStore.Application.Abstractions.Data;

namespace OnlineStore.Application.ProductImages;

public sealed record UpdateProductImageCommand(Guid ProductId, IFormFile File) : IRequest<string>;

public sealed class UpdateProductImageCommandHandler(IApplicationDbContext dbContext, IMinioService minioService)
		: IRequestHandler<UpdateProductImageCommand, string>
{
	public async Task<string> Handle(UpdateProductImageCommand request, CancellationToken ct)
	{
		var product = await dbContext.Products
				.Include(p => p.Image)
				.FirstOrDefaultAsync(p => p.Id == request.ProductId, ct);
		if (product is null || product.Image is null)
			throw new NotFoundException($"Image for product '{request.ProductId}' not found");

		await minioService.RemoveFileAsync(null, product.Image.ObjectName);

		var objectName = minioService.CreateObjectName(request.File.FileName);
		await minioService.UploadFileAsync(null, objectName, request.File.OpenReadStream(), request.File.ContentType);

		product.Image.ObjectName = objectName;
		await dbContext.SaveChangesAsync(ct);

		return await minioService.GetPresignedUrlAsync(null, objectName);
	}
}