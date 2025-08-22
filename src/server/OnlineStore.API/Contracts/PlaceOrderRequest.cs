using OnlineStore.Domain.Enums;

namespace OnlineStore.API.Contracts;

public record PlaceOrderRequest(
		string ShippingAddress,
		DeliveryMethod DeliveryMethod);