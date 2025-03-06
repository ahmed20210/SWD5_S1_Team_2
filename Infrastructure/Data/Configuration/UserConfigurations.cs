using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configuration;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.FName)
            .IsRequired().HasMaxLength(15);

        builder.Property(i => i.MainAddressID)
            .IsRequired();

        builder.Property(i => i.LName)
            .IsRequired().HasMaxLength(15);

         builder.Property(i => i.Role)
            .IsRequired().HasMaxLength(10);

        builder.Property(i => i.Phone)
            .IsRequired().HasMaxLength(11);

        builder.Property(i => i.Email)
            .IsRequired().HasMaxLength(20);

        builder.Property(i => i.Password)
            .IsRequired().HasMaxLength(20);










    }
}