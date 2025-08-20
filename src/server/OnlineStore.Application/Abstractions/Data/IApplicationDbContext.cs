using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Application.Abstractions.Data;

public interface IApplicationDbContext
{
	DbSet<User> Users { get; }
	DbSet<RefreshToken> RefreshTokens { get; }
	DbSet<PasswordResetToken> PasswordResetTokens { get; }
	DbSet<Category> Categories { get; }
	DbSet<Product> Products { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}