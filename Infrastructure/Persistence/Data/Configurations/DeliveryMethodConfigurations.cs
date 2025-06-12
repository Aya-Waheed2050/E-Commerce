using DomainLayer.Models.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
    internal class DeliveryMethodConfigurations : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {

            builder.ToTable("DeliveryMethods");

            builder.Property(d => d.Cost)
                   .HasColumnType("decimal(8,2)");

            builder.Property(d => d.ShortName)
                   .HasColumnType("varchar")
                   .HasMaxLength(50);
                   
            builder.Property(d => d.Description)
                   .HasColumnType("varchar")
                   .HasMaxLength(200);

            builder.Property(d => d.Description)
                 .HasColumnType("varchar")
                 .HasMaxLength(50);

        }
    }
}
