using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configuration;

public class ReviewConfigurations : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");

        builder.HasKey(i => i.Id);

        
        builder.Property(i => i.Comment)
            .HasMaxLength(100).HasDefaultValue("No Comment Available.");

        builder.Property(i => i.ReviewDate)
            .IsRequired().HasDefaultValueSql("GETDATE()");


       






    }
}