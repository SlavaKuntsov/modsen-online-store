namespace OnlineStore.Domain.Entities;

public class ProductImage
{
	public Guid Id { get; set; }
	public Guid ProductId { get; set; }
	public Product Product { get; set; } = null!;
	public string ObjectName { get; set; } = null!;
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}