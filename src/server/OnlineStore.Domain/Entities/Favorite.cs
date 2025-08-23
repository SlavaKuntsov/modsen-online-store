namespace OnlineStore.Domain.Entities;

public class Favorite
{
	public Guid Id { get; set; }
	public Guid UserId { get; set; }
	public Guid ProductId { get; set; }

	public User User { get; set; } = null!;
	public Product Product { get; set; } = null!;

	public Favorite() { }

	public Favorite(Guid userId, Guid productId)
	{
		Id = Guid.NewGuid();
		UserId = userId;
		ProductId = productId;
	}
}