using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;
using Domain;

namespace Infrastructure.Data.Configuration;

public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)

    {
       
        builder.ToTable("Coupons");

      
        builder.HasKey(c => c.Id);

        
        builder.HasIndex(c => c.Code).IsUnique();

   
        builder.Property(c => c.Code)
               .IsRequired()
               .HasMaxLength(50);

       
        builder.Property(c => c.Type)
               .HasConversion<string>()
               .IsRequired();

      
        builder.Property(c => c.Value)
               .IsRequired()
               .HasPrecision(18, 2);

        
        builder.Property(c => c.UsageLimit)
               .IsRequired();

        
        builder.Property(c => c.UsageCount)
               .HasDefaultValue(0);

        builder.Property(c => c.MaxDiscount)
               .HasPrecision(18, 2)
               .IsRequired(false);

        
        builder.Property(c => c.MinOrderValue)
               .IsRequired()
               .HasPrecision(18, 2);

       
        builder.Property(c => c.StartDate)
               .IsRequired();

       
        builder.Property(c => c.ExpiryDate)
               .IsRequired();

       
        builder.Property(c => c.Status)
               .HasConversion<string>()
               .IsRequired();


    }


}








