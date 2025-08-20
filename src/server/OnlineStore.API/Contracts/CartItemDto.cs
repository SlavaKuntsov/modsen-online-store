namespace OnlineStore.API.Contracts;

public record CartItemDto(Guid ProductId, string ProductName, decimal UnitPrice, int Quantity, decimal SubTotal);
