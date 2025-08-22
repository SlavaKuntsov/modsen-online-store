namespace OnlineStore.API.Contracts.Cart;

public record UpdateCartItemRequest(Guid ProductId, int Quantity);