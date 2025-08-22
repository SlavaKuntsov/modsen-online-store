using System.Linq;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Application.Dtos;

namespace OnlineStore.Application.Orders;

public sealed record GetOrdersQuery(Guid UserId) : IRequest<List<OrderDto>>;

public sealed class GetOrdersQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetOrdersQuery, List<OrderDto>>
{
    public async Task<List<OrderDto>> Handle(GetOrdersQuery request, CancellationToken ct)
    {
        var orders = await dbContext.Orders
            .Include(o => o.Items)
            .Where(o => o.UserId == request.UserId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(ct);

        return orders.Select(Map).ToList();
    }

    private static OrderDto Map(OnlineStore.Domain.Entities.Order order)
    {
        var items = order.Items
            .Select(i => new OrderItemDto(i.ProductId, i.ProductName, i.UnitPrice, i.Quantity, i.UnitPrice * i.Quantity))
            .ToList();
        return new OrderDto(order.Id, order.UserId, items, order.Total, order.DeliveryMethod, order.ShippingAddress, order.CreatedAt, order.Status);
    }
}
