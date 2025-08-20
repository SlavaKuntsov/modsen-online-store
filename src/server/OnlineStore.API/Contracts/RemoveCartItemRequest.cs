using System;
namespace OnlineStore.API.Contracts;

public record RemoveCartItemRequest(Guid ProductId, int Quantity);
