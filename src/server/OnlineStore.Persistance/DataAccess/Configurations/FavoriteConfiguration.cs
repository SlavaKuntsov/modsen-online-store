using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Persistance.DataAccess.Configurations;

public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
{
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
                builder.HasKey(f => f.Id);
                builder.HasIndex(f => new { f.UserId, f.ProductId }).IsUnique();
                builder.HasOne(f => f.User)
                        .WithMany()
                        .HasForeignKey(f => f.UserId);
                builder.HasOne(f => f.Product)
                        .WithMany()
                        .HasForeignKey(f => f.ProductId);
        }
}
