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
			.WithMany(p => p.Images)
			.HasForeignKey(pi => pi.ProductId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}