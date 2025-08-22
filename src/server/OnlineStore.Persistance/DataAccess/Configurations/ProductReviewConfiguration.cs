using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Persistance.DataAccess.Configurations;

public class ProductReviewConfiguration : IEntityTypeConfiguration<ProductReview>
{
        public void Configure(EntityTypeBuilder<ProductReview> builder)
        {
                builder.HasKey(r => r.Id);
                builder.Property(r => r.Rating).IsRequired();
                builder.HasOne(r => r.Product)
                        .WithMany()
                        .HasForeignKey(r => r.ProductId);
                builder.HasOne(r => r.User)
                        .WithMany()
                        .HasForeignKey(r => r.UserId);
        }
}
