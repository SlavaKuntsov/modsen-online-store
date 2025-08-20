using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Domain.Entities;

namespace OnlineStore.API.Services;

public class CartService(IMemoryCache cache, IHttpContextAccessor accessor, IApplicationDbContext dbContext) : ICartService
{
    private const string CartCookie = "CartId";

    public async Task<Cart> GetCartAsync()
    {
        var context = accessor.HttpContext!;
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userId = Guid.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var cart = await dbContext.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart is null)
            {
                cart = new Cart { UserId = userId };
                dbContext.Carts.Add(cart);
                await dbContext.SaveChangesAsync();
            }
            return cart;
        }

        var key = GetGuestCacheKey();
        if (!cache.TryGetValue(key, out Cart? guestCart))
        {
            guestCart = new Cart();
            cache.Set(key, guestCart, TimeSpan.FromHours(1));
        }
        return guestCart;
    }

    public async Task<Cart> AddItemAsync(Guid productId, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero");
        }

        var cart = await GetCartAsync();
        var product = await dbContext.Products.FindAsync(productId);
        if (product is null)
        {
            throw new ArgumentException("Product not found");
        }

        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        var newQuantity = (item?.Quantity ?? 0) + quantity;
        if (newQuantity > product.StockQuantity)
        {
            throw new InvalidOperationException("Insufficient stock");
        }

        if (item is null)
        {
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
            item.Quantity = newQuantity;
        }
        await SaveAsync(cart);
        return cart;
    }

    public async Task<Cart> RemoveItemAsync(Guid productId, int quantity)
    {
        var cart = await GetCartAsync();
        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item is not null)
        {
            if (quantity <= 0 || quantity >= item.Quantity)
            {
                cart.Items.Remove(item);
            }
            else
            {
                item.Quantity -= quantity;
            }
            await SaveAsync(cart);
        }
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
            var product = await dbContext.Products.FindAsync(productId);
            if (product is null)
            {
                throw new ArgumentException("Product not found");
            }
            if (quantity > product.StockQuantity)
            {
                throw new InvalidOperationException("Insufficient stock");
            }
            item.Quantity = quantity;
        }
        await SaveAsync(cart);
        return cart;
    }

    public async Task<Cart> ReplaceItemsAsync(IEnumerable<(Guid productId, int quantity)> items)
    {
        var cart = await GetCartAsync();
        var requested = items.ToList();
        var allowed = requested.Select(i => i.productId).ToHashSet();

        cart.Items.RemoveAll(i => !allowed.Contains(i.ProductId));

        foreach (var (productId, quantity) in requested)
        {
            if (quantity <= 0)
            {
                cart.Items.RemoveAll(i => i.ProductId == productId);
                continue;
            }

            var product = await dbContext.Products.FindAsync(productId);
            if (product is null)
            {
                throw new ArgumentException($"Product {productId} not found");
            }
            if (quantity > product.StockQuantity)
            {
                throw new InvalidOperationException("Insufficient stock");
            }
            var existing = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (existing is null)
            {
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
                existing.ProductName = product.Name;
                existing.UnitPrice = product.Price;
                existing.Quantity = quantity;
            }
        }

        await SaveAsync(cart);
        return cart;
    }

    private string GetGuestCacheKey()
    {
        var context = accessor.HttpContext!;
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

    private async Task SaveAsync(Cart cart)
    {
        if (cart.UserId.HasValue)
        {
            await dbContext.SaveChangesAsync();
            return;
        }

        var key = GetGuestCacheKey();
        cache.Set(key, cart, TimeSpan.FromHours(1));
    }
}
