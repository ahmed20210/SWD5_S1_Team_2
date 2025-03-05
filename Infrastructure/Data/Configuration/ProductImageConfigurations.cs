using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configuration;

public class ProductImageConfigurations : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable("ProductImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.ImageURL)
            .IsRequired().HasMaxLength(300);

        builder.Property(i => i.Data)
            .IsRequired().HasMaxLength(200);

        builder.Property(i => i.IsMain)
            .IsRequired().HasDefaultValue(true);

        builder.Property(i => i.CreatedAt)
            .IsRequired().HasDefaultValueSql("GETDATE()");


    }
}
