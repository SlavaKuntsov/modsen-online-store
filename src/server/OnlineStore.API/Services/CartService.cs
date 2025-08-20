using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Domain.Entities;

namespace OnlineStore.API.Services;

public class CartService(IMemoryCache cache, IHttpContextAccessor accessor, IApplicationDbContext dbContext) : ICartService
{
    private const string CartCookie = "CartId";

    public async Task<Cart> GetCartAsync()
    {
        var key = GetCacheKey();
        if (!cache.TryGetValue(key, out Cart? cart))
        {
            cart = new Cart();
            var context = accessor.HttpContext!;
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var id = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (Guid.TryParse(id, out var userId))
                {
                    cart.UserId = userId;
                }
            }
            cache.Set(key, cart, TimeSpan.FromHours(1));
        }
        return cart;
    }

    public async Task<Cart> AddItemAsync(Guid productId, int quantity)
    {
        var cart = await GetCartAsync();
        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item is null)
        {
            var product = await dbContext.Products.FindAsync(productId);
            if (product is null)
            {
                throw new ArgumentException("Product not found");
            }
            cart.Items.Add(new CartItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                UnitPrice = product.Price,
                Quantity = quantity
            });
        }
        else
        {
            item.Quantity += quantity;
        }
        Save(cart);
        return cart;
    }

    public async Task<Cart> RemoveItemAsync(Guid productId)
    {
        var cart = await GetCartAsync();
        cart.Items.RemoveAll(i => i.ProductId == productId);
        Save(cart);
        return cart;
    }

    public async Task<Cart> UpdateItemQuantityAsync(Guid productId, int quantity)
    {
        var cart = await GetCartAsync();
        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item is null)
        {
            return cart;
        }
        if (quantity <= 0)
        {
            cart.Items.Remove(item);
        }
        else
        {
            item.Quantity = quantity;
        }
        Save(cart);
        return cart;
    }

    private string GetCacheKey()
    {
        var context = accessor.HttpContext!;
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            return $"cart:user:{userId}";
        }
        if (!context.Request.Cookies.TryGetValue(CartCookie, out var cartId))
        {
            cartId = Guid.NewGuid().ToString();
            context.Response.Cookies.Append(CartCookie, cartId, new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });
        }
        return $"cart:guest:{cartId}";
    }

    private void Save(Cart cart)
    {
        var key = GetCacheKey();
        cache.Set(key, cart, TimeSpan.FromHours(1));
    }
}
