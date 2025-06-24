namespace OnlineStore.Domain.Entities;

public class RefreshToken
{
	public Guid Id { get; set; }
	public Guid? UserId { get; set; }
	public string Token { get; set; }
	public DateTime ExpiresAt { get; set; }
	public DateTime CreatedAt { get; set; }
	public bool IsRevoked { get; set; }

	public virtual User User { get; set; } = null!;

	public RefreshToken(
		Guid userId,
		string token,
		int refreshTokenExpirationDays)
	{
		Id = Guid.NewGuid();
		Token = token;
		ExpiresAt = DateTime.UtcNow.Add(TimeSpan.FromDays(refreshTokenExpirationDays));
		CreatedAt = DateTime.UtcNow;
		IsRevoked = false;
		UserId = userId;
	}
}