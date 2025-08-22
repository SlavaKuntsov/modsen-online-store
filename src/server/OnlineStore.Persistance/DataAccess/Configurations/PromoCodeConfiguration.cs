using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Persistance.DataAccess.Configurations;

public class PromoCodeConfiguration : IEntityTypeConfiguration<PromoCode>
{
        public void Configure(EntityTypeBuilder<PromoCode> builder)
        {
                builder.HasKey(p => p.Id);
                builder.Property(p => p.Code).IsRequired();
        }
}
