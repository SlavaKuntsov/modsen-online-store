using OnlineStore.Domain.Entities;

namespace OnlineStore.Application.Carts;

public interface ICartService
{
	Task<Cart> GetCartAsync();
	Task<Cart> AddItemAsync(Guid productId, int quantity);
	Task<Cart> RemoveItemAsync(Guid productId, int quantity);
	Task<Cart> UpdateItemQuantityAsync(Guid productId, int quantity);
	Task<Cart> ReplaceItemsAsync(IEnumerable<(Guid productId, int quantity)> items);
}