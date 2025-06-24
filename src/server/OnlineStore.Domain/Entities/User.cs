using OnlineStore.Domain.Enums;

namespace OnlineStore.Domain.Entities;

public class User
{
	public Guid Id { get; set; }
	public string Email { get; set; }
	public string PasswordHash { get; set; }
	public Role Role { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string DateOfBirth { get; set; }
	public DateTimeOffset CreatedAt { get; set; }

	public virtual RefreshToken RefreshToken { get; set; } = null!;

	public User(
		string email,
		string passwordHash,
		Role role,
		string firstName,
		string lastName,
		string dateOfBirth)
	{
		Id = Guid.NewGuid();
		Email = email;
		PasswordHash = passwordHash;
		Role = role;
		FirstName = firstName;
		LastName = lastName;
		DateOfBirth = dateOfBirth;
		CreatedAt = DateTimeOffset.UtcNow;
	}
}