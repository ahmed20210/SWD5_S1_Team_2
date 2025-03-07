using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configuration;

public class BoosterConfigurations : IEntityTypeConfiguration<Booster>
{
    public void Configure(EntityTypeBuilder<Booster> builder)
    {
        builder.ToTable("Boosters");

        builder.HasKey(i => i.ID);

        builder.Property(i => i.Title)
            .IsRequired().HasMaxLength(50);

        builder.Property(i => i.Description)
            .HasMaxLength(200).HasDefaultValue("No Description Available.");

        builder.Property(i => i.ImagePath)
            .IsRequired().HasMaxLength(300);

        builder.Property(i => i.BoosterURL)
           .IsRequired().HasMaxLength(300);

        builder.Property(i => i.BoosterType)
          .IsRequired().HasMaxLength(20);






    }
}