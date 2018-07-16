using eShop.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.DataAccess.EntityFramework.Configurations
{
    // Note that this class only exists as an example.
    // It is not needed because we followed the EF naming conventions.
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasOne<Customer>();
            builder.HasMany<OrderItem>();
        }
    }
}
