using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Persistance.DataAccess.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
	public void Configure(EntityTypeBuilder<Order> builder)
	{
		builder.HasKey(o => o.Id);

		builder.Property(o => o.UserId)
				.IsRequired(false);

		builder.Property(o => o.ShippingAddress).IsRequired();

		builder.Property(o => o.DeliveryMethod)
				.HasConversion<int>();

		builder.Property(o => o.Status)
				.HasConversion<int>();

		builder.OwnsMany(o => o.Items, itemBuilder =>
		{
			itemBuilder.WithOwner().HasForeignKey("order_id");
			itemBuilder.HasKey("order_id", nameof(OrderItem.ProductId));

			itemBuilder.Property(i => i.ProductId).IsRequired();
			itemBuilder.Property(i => i.ProductName).IsRequired();
			itemBuilder.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)");
			itemBuilder.Property(i => i.Quantity).IsRequired();

			itemBuilder.ToTable("order_items");
		});

		builder.ToTable("orders");
	}
}