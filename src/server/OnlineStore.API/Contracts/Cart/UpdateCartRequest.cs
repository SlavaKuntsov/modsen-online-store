namespace OnlineStore.API.Contracts.Cart;

public record UpdateCartRequest(List<UpdateCartItemRequest> Items);