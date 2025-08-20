using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Application.Dtos;

namespace OnlineStore.Application.Products;

public sealed record GetProductsQuery(
    Guid? CategoryId = null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    double? MinRating = null,
    bool? InStock = null,
    string? SortBy = null,
    bool Descending = false) : IRequest<List<ProductDto>>;

public sealed class GetProductsQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetProductsQuery, List<ProductDto>>
{
    public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken ct)
    {
        IQueryable<Domain.Entities.Product> query = dbContext.Products.AsQueryable();

        if (request.CategoryId.HasValue)
            query = query.Where(p => p.CategoryId == request.CategoryId.Value);

        if (request.MinPrice.HasValue)
            query = query.Where(p => p.Price >= request.MinPrice.Value);

        if (request.MaxPrice.HasValue)
            query = query.Where(p => p.Price <= request.MaxPrice.Value);

        if (request.MinRating.HasValue)
            query = query.Where(p => p.Rating >= request.MinRating.Value);

        if (request.InStock.HasValue)
        {
            if (request.InStock.Value)
                query = query.Where(p => p.StockQuantity > 0);
            else
                query = query.Where(p => p.StockQuantity == 0);
        }

        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = request.SortBy.ToLower() switch
            {
                "price" => request.Descending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                "popularity" => request.Descending ? query.OrderByDescending(p => p.Popularity) : query.OrderBy(p => p.Popularity),
                "createdat" => request.Descending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt),
                _ => query
            };
        }

        var products = await query.ToListAsync(ct);

        return products.Select(p => new ProductDto(
            p.Id,
            p.Name,
            p.Description,
            p.Price,
            p.StockQuantity,
            p.CategoryId,
            p.Rating,
            p.Popularity,
            p.CreatedAt)).ToList();
    }
}
