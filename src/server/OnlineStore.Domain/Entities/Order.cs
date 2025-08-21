using OnlineStore.Domain.Enums;

namespace OnlineStore.Domain.Entities;

public class Order
{
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public DeliveryMethod DeliveryMethod { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public decimal Total => Items.Sum(i => i.UnitPrice * i.Quantity);

        public Order() { }

        public Order(Guid userId, DeliveryMethod deliveryMethod, string shippingAddress, List<OrderItem> items)
        {
                Id = Guid.NewGuid();
                UserId = userId;
                DeliveryMethod = deliveryMethod;
                ShippingAddress = shippingAddress;
                Items = items;
                CreatedAt = DateTime.UtcNow;
        }
}
