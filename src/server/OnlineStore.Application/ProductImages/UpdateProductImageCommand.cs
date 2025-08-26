using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Minios.Services;
using OnlineStore.Application.Abstractions.Data;

namespace OnlineStore.Application.ProductImages;

public sealed record UpdateProductImageCommand(Guid ImageId, IFormFile File) : IRequest<string>;

public sealed class UpdateProductImageCommandHandler(IApplicationDbContext dbContext, IMinioService minioService)
		: IRequestHandler<UpdateProductImageCommand, string>
{
	public async Task<string> Handle(UpdateProductImageCommand request, CancellationToken ct)
	{
		var image = await dbContext.ProductImages
			.FirstOrDefaultAsync(i => i.Id == request.ImageId, ct);
		if (image is null)
			throw new NotFoundException($"Image with id '{request.ImageId}' not found");

		await minioService.RemoveFileAsync(null, image.ObjectName);

		var objectName = minioService.CreateObjectName(request.File.FileName);
		await minioService.UploadFileAsync(null, objectName, request.File.OpenReadStream(), request.File.ContentType);

		image.ObjectName = objectName;
		await dbContext.SaveChangesAsync(ct);

		return await minioService.GetPresignedUrlAsync(null, objectName);
	}
}