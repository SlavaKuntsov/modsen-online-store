namespace OnlineStore.API.Contracts;

public record UpdateCartItemRequest(Guid ProductId, int Quantity);