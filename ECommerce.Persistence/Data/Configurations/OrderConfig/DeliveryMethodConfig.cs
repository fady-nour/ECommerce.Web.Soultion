using ECommerce.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Data.Configurations.OrderConfig
{
    public class DeliveryMethodConfig : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.ToTable("DeliveryMethods");
            builder.Property(a=>a.Price).HasColumnType("decimal(8,2)");
            builder.Property(a=>a.ShortName).HasColumnType("varchar").HasMaxLength(50);
            builder.Property(a=>a.Description).HasColumnType("varchar").HasMaxLength(100);
            builder.Property(a=>a.DeliveryTime).HasColumnType("varchar").HasMaxLength(50);
        }
    }
}
