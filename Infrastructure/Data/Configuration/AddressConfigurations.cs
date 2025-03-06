using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configuration;

public class AddressConfigurations : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Addresses");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.City)
            .IsRequired().HasMaxLength(20);

        builder.Property(i => i.ZipCode)
            .IsRequired().HasMaxLength(10);

        builder.Property(i => i.State)
            .IsRequired().HasMaxLength(20);

        builder.Property(i => i.Street)
            .IsRequired().HasMaxLength(20);

        builder.Property(i => i.Country)
            .IsRequired().HasMaxLength(20);



       




    }
}