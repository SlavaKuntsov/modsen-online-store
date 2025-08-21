using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Dtos;

public record OrderDto(
		Guid Id,
		Guid UserId,
		List<OrderItemDto> Items,
		decimal Total,
		DeliveryMethod DeliveryMethod,
		string ShippingAddress,
		DateTime CreatedAt);