namespace OnlineStore.Domain.Entities;

public class ProductReview
{
	public Guid Id { get; set; }
	public Guid ProductId { get; set; }
	public Guid UserId { get; set; }
	public int Rating { get; set; }
	public string Comment { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; }

	public Product Product { get; set; } = null!;
	public User User { get; set; } = null!;

	public ProductReview() { }

	public ProductReview(Guid productId, Guid userId, int rating, string comment)
	{
		Id = Guid.NewGuid();
		ProductId = productId;
		UserId = userId;
		Rating = rating;
		Comment = comment;
		CreatedAt = DateTime.UtcNow;
	}
}