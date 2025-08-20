namespace OnlineStore.Domain.Entities;

public class CartItem
{
	public Guid ProductId { get; set; }
	public string ProductName { get; set; } = string.Empty;
	public decimal UnitPrice { get; set; }
	public int Quantity { get; set; }
}