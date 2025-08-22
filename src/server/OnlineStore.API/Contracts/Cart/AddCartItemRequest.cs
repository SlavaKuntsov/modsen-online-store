namespace OnlineStore.API.Contracts.Cart;

public record AddCartItemRequest(Guid ProductId, int Quantity);