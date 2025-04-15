using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configuration;

public class ProductConfigurations : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        
        builder.Property(p => p.Status)
            .HasConversion<string>();
        
        
        builder.HasMany(p => p.Reviews)
            .WithOne()
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Images)
            .WithOne()
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Discount)
            .WithOne()
            .HasForeignKey<Product>(p => p.DiscountId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(p => p.PastDiscountList)
            .WithOne(d => d.Product)
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}