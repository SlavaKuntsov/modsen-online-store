using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Application.Dtos;

namespace OnlineStore.Application.Products;

public sealed record GetProductByIdQuery(Guid Id) : IRequest<ProductDto>;

public sealed class GetProductByIdQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken ct)
    {
        var product = await dbContext.Products
            .FirstOrDefaultAsync(p => p.Id == request.Id, ct);

        if (product is null)
            throw new NotFoundException($"Product with id '{request.Id}' not found");

        return new ProductDto(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.StockQuantity,
            product.CategoryId,
            product.Rating,
            product.Popularity,
            product.CreatedAt);
    }
}
