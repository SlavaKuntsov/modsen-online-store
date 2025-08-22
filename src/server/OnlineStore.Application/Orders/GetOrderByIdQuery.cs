using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Application.Dtos;

namespace OnlineStore.Application.Orders;

public sealed record GetOrderByIdQuery(Guid OrderId, Guid UserId) : IRequest<OrderDto>;

public sealed class GetOrderByIdQueryHandler(IApplicationDbContext dbContext)
	: IRequestHandler<GetOrderByIdQuery, OrderDto>
{
	public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken ct)
	{
		var order = await dbContext.Orders
			.Include(o => o.Items)
			.FirstOrDefaultAsync(o => o.Id == request.OrderId && o.UserId == request.UserId, ct);

		if (order is null)
			throw new NotFoundException($"Order {request.OrderId} not found");

		var items = order.Items
			.Select(i => new OrderItemDto(i.ProductId, i.ProductName, i.UnitPrice, i.Quantity, i.UnitPrice * i.Quantity))
			.ToList();

		return new OrderDto(order.Id, order.UserId, items, order.Total, order.DeliveryMethod, order.ShippingAddress, order.CreatedAt, order.Status);
	}
}