using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Application.Abstractions.Data;

public interface IApplicationDbContext
{
	DbSet<User> Users { get; set; }
	DbSet<RefreshToken> RefreshTokens { get; set; }
	DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
	
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}