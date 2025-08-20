using OnlineStore.Domain.Entities;

namespace OnlineStore.API.Services;

public interface ICartService
{
    Task<Cart> GetCartAsync();
    Task<Cart> AddItemAsync(Guid productId, int quantity);
    Task<Cart> RemoveItemAsync(Guid productId);
    Task<Cart> UpdateItemQuantityAsync(Guid productId, int quantity);
}
