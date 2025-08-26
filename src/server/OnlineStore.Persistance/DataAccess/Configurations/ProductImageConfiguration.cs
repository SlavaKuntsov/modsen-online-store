using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Persistance.DataAccess.Configurations;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
	public void Configure(EntityTypeBuilder<ProductImage> builder)
	{
		builder.HasKey(pi => pi.Id);
		builder.Property(pi => pi.ObjectName).IsRequired();

                builder.HasOne(pi => pi.Product)
                        .WithOne(p => p.Image)
                        .HasForeignKey<ProductImage>(pi => pi.ProductId)
                        .OnDelete(DeleteBehavior.Cascade);

                builder.HasIndex(pi => pi.ProductId).IsUnique();
        }
}