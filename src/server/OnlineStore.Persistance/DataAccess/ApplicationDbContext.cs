using System.Reflection;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Persistance.DataAccess;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: DbContext(options), IApplicationDbContext
{
	public DbSet<User> Users { get; set; }
	public DbSet<RefreshToken> RefreshTokens { get; set; }
	public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
	public DbSet<Category> Categories { get; set; }
	public DbSet<Product> Products { get; set; }
	public DbSet<Cart> Carts { get; set; }
	public DbSet<Order> Orders { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		base.OnModelCreating(modelBuilder);
	}
}