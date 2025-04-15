using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;
using Domain;

namespace Infrastructure.Data.Configuration;

public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)

    {
        builder.Property(c => c.Type)
            .HasConversion<string>();
        builder.HasOne(c => c.Banner)
            .WithOne(b => b.Coupon)
            .HasForeignKey<Banner>(c => c.CouponId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}