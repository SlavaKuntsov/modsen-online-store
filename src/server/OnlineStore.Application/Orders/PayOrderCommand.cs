using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Application.Dtos;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Orders;

public sealed record PayOrderCommand(Guid OrderId) : IRequest<OrderDto>;

public sealed class PayOrderCommandHandler(IApplicationDbContext dbContext)
		: IRequestHandler<PayOrderCommand, OrderDto>
{
	public async Task<OrderDto> Handle(PayOrderCommand request, CancellationToken ct)
	{
		var order = await dbContext.Orders
						.Include(o => o.Items)
						.Include(o => o.PromoCode)
						.FirstOrDefaultAsync(o => o.Id == request.OrderId, ct);
		if (order is null)
			throw new NotFoundException($"Order {request.OrderId} not found");

		order.Status = OrderStatus.Completed;
		await dbContext.SaveChangesAsync(ct);

		var items = order.Items.Select(i => new OrderItemDto(i.ProductId, i.ProductName, i.UnitPrice, i.Quantity, i.UnitPrice * i.Quantity)).ToList();

		return new OrderDto(order.Id, order.UserId, items, order.Total, order.DiscountAmount, order.FinalTotal, order.DeliveryMethod, order.ShippingAddress, order.CreatedAt, order.Status, order.PromoCode?.Code);
	}
}