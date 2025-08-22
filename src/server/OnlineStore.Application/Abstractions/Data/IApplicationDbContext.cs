using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Application.Abstractions.Data;

public interface IApplicationDbContext
{
	DbSet<User> Users { get; set; }
	DbSet<RefreshToken> RefreshTokens { get; set; }
	DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
	DbSet<Category> Categories { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<Cart> Carts { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<PromoCode> PromoCodes { get; set; }
        DbSet<ProductReview> ProductReviews { get; set; }
        DbSet<Favorite> Favorites { get; set; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}