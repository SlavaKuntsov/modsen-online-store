namespace OnlineStore.API.Contracts.Cart;

public record CartDto(List<CartItemDto> Items, decimal Total);