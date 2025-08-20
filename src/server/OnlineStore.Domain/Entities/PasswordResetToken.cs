namespace OnlineStore.Domain.Entities;

public class PasswordResetToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; }

    public virtual User User { get; set; } = null!;

    public PasswordResetToken() { }

    public PasswordResetToken(
        Guid userId,
        string token,
        int resetTokenExpirationMinutes)
    {
        Id = Guid.NewGuid();
        Token = token;
        ExpiresAt = DateTime.UtcNow.Add(TimeSpan.FromMinutes(resetTokenExpirationMinutes));
        CreatedAt = DateTime.UtcNow;
        UserId = userId;
        IsUsed = false;
    }
}