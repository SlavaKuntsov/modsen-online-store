using MediatR;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Application.Dtos;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Application.Products;

public sealed record CreateProductCommand(
	string Name,
	string Description,
	decimal Price,
	int StockQuantity,
	Guid CategoryId) : IRequest<ProductDto>;

public sealed class CreateProductCommandHandler(IApplicationDbContext dbContext)
	: IRequestHandler<CreateProductCommand, ProductDto>
{
	public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken ct)
	{
		var product = new Product(
			request.Name,
			request.Description,
			request.Price,
			request.StockQuantity,
			request.CategoryId);

		await dbContext.Products.AddAsync(product, ct);
		await dbContext.SaveChangesAsync(ct);

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
												null);
	}
}