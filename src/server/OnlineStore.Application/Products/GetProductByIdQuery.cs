using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Minios.Services;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Application.Dtos;

namespace OnlineStore.Application.Products;

public sealed record GetProductByIdQuery(Guid Id) : IRequest<ProductDto>;

public sealed class GetProductByIdQueryHandler(IApplicationDbContext dbContext, IMinioService minioService)
		: IRequestHandler<GetProductByIdQuery, ProductDto>
{
	public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken ct)
	{
                var product = await dbContext.Products
                                .Include(p => p.Image)
                                .FirstOrDefaultAsync(p => p.Id == request.Id, ct);

		if (product is null)
			throw new NotFoundException($"Product with id '{request.Id}' not found");

                string? url = null;
                if (product.Image is not null)
                        url = await minioService.GetPresignedUrlAsync(null, product.Image.ObjectName);

                return new ProductDto(
                                product.Id,
                                product.Name,
                                product.Description,
                                product.Price,
                                product.StockQuantity,
                                product.CategoryId,
                                product.Rating,
                                product.Popularity,
                                product.CreatedAt,
                                url);
        }
}