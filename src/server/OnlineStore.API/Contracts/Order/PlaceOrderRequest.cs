using OnlineStore.Domain.Enums;

namespace OnlineStore.API.Contracts.Order;

public record PlaceOrderRequest(
                string ShippingAddress,
                DeliveryMethod DeliveryMethod,
                string? PromoCode);