using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Application.Dtos;

namespace OnlineStore.Application.Products;

public sealed record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    Guid CategoryId) : IRequest<ProductDto>;

public sealed class UpdateProductCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<UpdateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken ct)
    {
        var product = await dbContext.Products
            .FirstOrDefaultAsync(p => p.Id == request.Id, ct);

        if (product is null)
            throw new NotFoundException($"Product with id '{request.Id}' not found");

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.StockQuantity = request.StockQuantity;
        product.CategoryId = request.CategoryId;

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
            product.CreatedAt);
    }
}
