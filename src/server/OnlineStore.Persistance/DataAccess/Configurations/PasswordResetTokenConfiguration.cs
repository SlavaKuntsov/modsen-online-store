using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Persistance.DataAccess.Configurations;

public class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
{
	public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
	{
		builder.HasKey(r => r.Id);

		builder.Property(r => r.Id)
			.IsRequired();

		builder.Property(r => r.UserId)
			.IsRequired();

		builder.Property(r => r.Token)
			.IsRequired()
			.HasMaxLength(500);

		builder.Property(r => r.ExpiresAt)
			.IsRequired();

		builder.Property(r => r.CreatedAt)
			.IsRequired();

		builder.Property(r => r.IsUsed)
			.IsRequired();

		builder.HasOne(t => t.User)
			.WithMany()
			.HasForeignKey(t => t.UserId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}