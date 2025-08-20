namespace OnlineStore.API.Contracts;

public record UpdateCartRequest(List<UpdateCartItemRequest> Items);