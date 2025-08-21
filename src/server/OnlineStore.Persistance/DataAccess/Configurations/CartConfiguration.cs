using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Persistance.DataAccess.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
                builder.HasKey(c => c.Id);

                builder.OwnsMany(c => c.Items, itemBuilder =>
                {
                        itemBuilder.WithOwner().HasForeignKey("CartId");
                        itemBuilder.HasKey("CartId", nameof(CartItem.ProductId));

                        itemBuilder.Property(i => i.ProductId).IsRequired();
                        itemBuilder.Property(i => i.ProductName).IsRequired();
                        itemBuilder.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)");
                        itemBuilder.Property(i => i.Quantity).IsRequired();

                        itemBuilder.ToTable("CartItems");
                });

                builder.ToTable("Carts");
        }
}
