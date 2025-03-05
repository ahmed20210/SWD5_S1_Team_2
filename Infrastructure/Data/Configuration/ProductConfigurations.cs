using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configuration;

public class ProductConfigurations : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Name)
            .IsRequired().HasMaxLength(30);
        
        builder.Property(i => i.Description)
            .IsRequired().HasMaxLength(200);

        builder.Property(i => i.Quantity)
            .IsRequired();


        builder.Property(i => i.Price)
            .IsRequired()
            .HasPrecision(8, 2);

        builder.Property(i => i.Status)
            .IsRequired();

        builder.Property(i => i.CountOfViews)
            .IsRequired().HasDefaultValue(0);

        builder.Property(i => i.CountOfPurchase)
            .IsRequired().HasDefaultValue(0);

        builder.Property(i => i.CountOfReviews)
            .IsRequired().HasDefaultValue(0);



    }
}
