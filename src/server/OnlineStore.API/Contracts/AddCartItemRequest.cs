namespace OnlineStore.API.Contracts;

public record AddCartItemRequest(Guid ProductId, int Quantity);