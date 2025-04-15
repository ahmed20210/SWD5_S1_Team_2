using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configuration;

public class OrderConfigurations : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(o => o.Status)
            .HasConversion<string>();
        
        builder.HasOne(o => o.Address)
            .WithMany()
            .HasForeignKey(o => o.AddressId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(o => o.OrderItems)
            .WithOne()
            .HasForeignKey(o => o.OrderId);
        
        builder.HasOne(o => o.Payment)
            .WithOne()
            .HasForeignKey<Order>(o => o.PaymentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(o => o.OrderTimeLines)
            .WithOne()
            .HasForeignKey(otl => otl.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(o => o.Coupon)
            .WithMany()
            .HasForeignKey(o => o.CouponId)
            .OnDelete(DeleteBehavior.Restrict);
        
        
    }
}