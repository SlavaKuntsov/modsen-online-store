using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Infrastructure.DataAccess.Configurations;


public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.HasKey(u => u.Id);

		builder.Property(u => u.Email)
			.IsRequired()
			.HasMaxLength(256);

		builder.Property(u => u.PasswordHash)
			.IsRequired();

		builder.Property(u => u.Role)
			.HasConversion(
				role => role.ToString(),
				value => Enum.Parse<Role>(value)
			);

		builder.Property(d => d.FirstName)
			.IsRequired()
			.HasMaxLength(100);

		builder.Property(d => d.LastName)
			.IsRequired()
			.HasMaxLength(100);

		builder.Property(d => d.DateOfBirth)
			.IsRequired();

		builder.HasOne(u => u.RefreshToken)
			.WithOne(r => r.User)
			.HasForeignKey<RefreshToken>(r => r.UserId);

		builder.HasData(
			new User
			{
				Id = Guid.NewGuid(),
				Email = "admin@email.com",
				PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword("qweQWE123"),
				DateOfBirth = DateOnly.MinValue.ToString(),
				Role = Role.Admin,
				FirstName = "admin",
				LastName = "admin"
			});
	}
}