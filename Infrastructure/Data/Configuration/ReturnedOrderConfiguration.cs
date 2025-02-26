using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configuration;

public class ReturnedOrderConfiguration : IEntityTypeConfiguration<ReturnedOrder>
{
    public void Configure(EntityTypeBuilder<ReturnedOrder> builder)
    {
        builder.ToTable("ReturnedOrders");

        builder.HasKey(ro => ro.Id);

        builder.Property(ro => ro.OrderID)
            .IsRequired();

        builder.Property(ro => ro.ProductID)
            .IsRequired();

        builder.Property(ro => ro.Quantity)
            .HasDefaultValue(1);

        builder.Property(ro => ro.Reason)
            .HasMaxLength(255);

        builder.Property(ro => ro.Description)
            .HasMaxLength(1000);

        builder.Property(ro => ro.CreatedAt)
            .HasDefaultValueSql("GETDATE()"); 

        builder.Property(ro => ro.RefundAmount)
            .IsRequired();

        builder.Property(ro => ro.RefundStatus)
            .IsRequired()
            .HasConversion<string>();
        
        // builder.HasIndex(ro => ro.OrderID);
        // builder.HasIndex(ro => ro.ProductID);
    }
}