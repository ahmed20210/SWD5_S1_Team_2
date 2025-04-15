using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
     

        builder.Property(p => p.Method)
            .IsRequired()
            .HasConversion<string>();
        
        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<string>();

    }
}