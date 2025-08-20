namespace OnlineStore.API.Contracts;

public record CartDto(List<CartItemDto> Items, decimal Total);
