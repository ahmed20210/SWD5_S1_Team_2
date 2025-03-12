using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configuration;

public class DiscountConfigurations : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.ToTable("Discount");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.ProductId)
            .IsRequired(false);

        builder.Property(d => d.OrderId)
            .IsRequired(false);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasDefaultValue(string.Empty);

        builder.Property(d => d.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(d => d.StartDate)
            .IsRequired();

        builder.Property(d => d.EndDate)
            .IsRequired();
        
        builder.HasMany(d => d.Orders) 
            .WithOne(o => o.Discount) 
            .HasForeignKey(o => o.DiscountId) 
            .IsRequired(false); 
        builder.HasMany(d => d.Products)
            .WithOne(p => p.Discount)
            .HasForeignKey(p => p.DiscountId)
            .IsRequired(false);
        builder
            .HasOne(d => d.Booster)
            .WithOne(b => b.Discount)
            .HasForeignKey<Discount>(d => d.BoosterId)
            .IsRequired(false);
    }
}