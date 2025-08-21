using FluentValidation;
using MediatR;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Application.Carts;
using OnlineStore.Application.Dtos;
using OnlineStore.Domain.Entities;
using OnlineStore.Domain.Enums;
using Utilities.Services;

namespace OnlineStore.Application.Orders;

public sealed record PlaceOrderCommand(
		string ShippingAddress,
		DeliveryMethod DeliveryMethod) : IRequest<OrderDto>;

public sealed class PlaceOrderCommandHandler(
		IApplicationDbContext dbContext,
		ICartService cartService,
		IEmailService emailService,
		IValidator<PlaceOrderCommand> validator)
		: IRequestHandler<PlaceOrderCommand, OrderDto>
{
	public async Task<OrderDto> Handle(PlaceOrderCommand request, CancellationToken ct)
	{
		var validationResult = await validator.ValidateAsync(request, ct);
		if (!validationResult.IsValid)
			throw new ValidationException(validationResult.Errors);

		var cart = await cartService.GetCartAsync();
		if (cart.UserId is null)
			throw new InvalidOperationException("User not authenticated");
		if (cart.Items.Count == 0)
			throw new InvalidOperationException("Cart is empty");

		var user = await dbContext.Users.FindAsync(new object[] { cart.UserId.Value }, ct);
		if (user is null)
			throw new InvalidOperationException("User not found");

		var items = cart.Items.Select(i => new OrderItem
		{
			ProductId = i.ProductId,
			ProductName = i.ProductName,
			UnitPrice = i.UnitPrice,
			Quantity = i.Quantity
		}).ToList();

		var order = new Order(cart.UserId.Value, request.DeliveryMethod, request.ShippingAddress, items);
		await dbContext.Orders.AddAsync(order, ct);

		cart.Items.Clear();
		await dbContext.SaveChangesAsync(ct);

		await emailService.SendEmailAsync(
				user.Email,
				"Order Confirmation",
				$"Your order {order.Id} has been placed.");

		var dtoItems = order.Items.Select(i => new OrderItemDto(i.ProductId, i.ProductName, i.UnitPrice, i.Quantity, i.UnitPrice * i.Quantity)).ToList();

		return new OrderDto(order.Id, order.UserId, dtoItems, order.Total, order.DeliveryMethod, order.ShippingAddress, order.CreatedAt);
	}
}