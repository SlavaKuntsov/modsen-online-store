namespace OnlineStore.API.Contracts.Cart;

public record CartItemDto(Guid ProductId, string ProductName, decimal UnitPrice, int Quantity, decimal SubTotal);