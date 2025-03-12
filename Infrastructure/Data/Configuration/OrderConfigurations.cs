using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configuration;

public class OrderConfigurations : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Order");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.CustomerId)
            .IsRequired();

        builder.Property(o => o.CouponId)
            .IsRequired(false);

        builder.Property(o => o.AddressId)
            .IsRequired();

        builder.Property(o => o.PhoneNumber)
            .HasMaxLength(15); 

        builder.Property(o => o.OrderDate)
            .IsRequired();

        builder.Property(o => o.TotalAmount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(o => o.Note)
            .HasMaxLength(500);

        builder.Property(o => o.Status)
            .IsRequired()
            .HasConversion<string>() 
            .HasMaxLength(20); 
        

    }
}