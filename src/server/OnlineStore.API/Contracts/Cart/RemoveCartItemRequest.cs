namespace OnlineStore.API.Contracts.Cart;

public record RemoveCartItemRequest(Guid ProductId, int Quantity);