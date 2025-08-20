using System.Linq;

namespace OnlineStore.Domain.Entities;

public class Cart
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? UserId { get; set; }
    public List<CartItem> Items { get; set; } = new();
    public decimal Total => Items.Sum(i => i.UnitPrice * i.Quantity);
}
