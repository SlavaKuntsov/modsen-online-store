using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Application.Carts;
using OnlineStore.Application.Dtos;
using OnlineStore.Domain.Entities;
using OnlineStore.Domain.Enums;
using Utilities.Services;

namespace OnlineStore.Application.Orders;

public sealed record PlaceOrderCommand(
				string ShippingAddress,
				DeliveryMethod DeliveryMethod,
				string? PromoCode) : IRequest<OrderDto>;

public sealed class PlaceOrderCommandHandler(
		IApplicationDbContext dbContext,
                ICartService cartService,
                IEmailQueueService emailQueueService,
                IValidator<PlaceOrderCommand> validator)
                : IRequestHandler<PlaceOrderCommand, OrderDto>
{
	public async Task<OrderDto> Handle(PlaceOrderCommand request, CancellationToken ct)
	{
		var validationResult = await validator.ValidateAsync(request, ct);
		if (!validationResult.IsValid)
			throw new ValidationException(validationResult.Errors);

		var cart = await cartService.GetCartAsync();
		if (cart.Items.Count == 0)
			throw new InvalidOperationException("Cart is empty");

		User? user = null;
		if (cart.UserId.HasValue)
		{
			user = await dbContext.Users.FindAsync([cart.UserId.Value], ct);
			if (user is null)
				throw new InvalidOperationException("User not found");
		}

		var items = new List<OrderItem>();
		foreach (var cartItem in cart.Items)
		{
			var product = await dbContext.Products.FindAsync([cartItem.ProductId], ct);
			if (product is null)
				throw new InvalidOperationException($"Product {cartItem.ProductId} not found");
			if (product.StockQuantity < cartItem.Quantity)
				throw new InvalidOperationException("Insufficient stock");

			product.StockQuantity -= cartItem.Quantity;

			items.Add(new OrderItem
			{
				ProductId = cartItem.ProductId,
				ProductName = cartItem.ProductName,
				UnitPrice = cartItem.UnitPrice,
				Quantity = cartItem.Quantity
			});
		}

		PromoCode? promo = null;
		decimal discount = 0;
		if (!string.IsNullOrWhiteSpace(request.PromoCode))
		{
			promo = await dbContext.PromoCodes
					.FirstOrDefaultAsync(p => p.Code == request.PromoCode && p.IsActive && p.ExpirationDate >= DateTime.UtcNow, ct);
			if (promo is null)
				throw new InvalidOperationException("Invalid promo code");
			discount = items.Sum(i => i.UnitPrice * i.Quantity) * (promo.DiscountPercentage / 100m);
		}

		var order = new Order(cart.UserId, request.DeliveryMethod, request.ShippingAddress, items)
		{
			PromoCodeId = promo?.Id,
			DiscountAmount = discount
		};
		await dbContext.Orders.AddAsync(order, ct);

		cart.Items.Clear();
		await dbContext.SaveChangesAsync(ct);

                if (user is not null)
                {
                        await emailQueueService.EnqueueEmailAsync(
                                user.Email,
                                "Order Confirmation",
                                $"Your order {order.Id} has been placed.");
                }

		var dtoItems = order.Items.Select(i => new OrderItemDto(i.ProductId, i.ProductName, i.UnitPrice, i.Quantity, i.UnitPrice * i.Quantity)).ToList();

		return new OrderDto(order.Id, order.UserId, dtoItems, order.Total, order.DiscountAmount, order.FinalTotal, order.DeliveryMethod, order.ShippingAddress, order.CreatedAt, order.Status, promo?.Code);
	}
}