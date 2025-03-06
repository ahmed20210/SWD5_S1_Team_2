using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configuration;

public class CategoryConfigurations : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Name)
            .IsRequired().HasMaxLength(50);


       
        builder.Property(i => i.Description)
            .HasMaxLength(100).HasDefaultValue("No Description Available.");

        builder.Property(i => i.ImagePath)
            .IsRequired().HasMaxLength(300);

        

       
       
    }
}