using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;
using Domain;

namespace Infrastructure.Data.Configuration;

public class LogConfiguration : IEntityTypeConfiguration<Log>
{
    public void Configure(EntityTypeBuilder<Log> builder)

    {
        
        builder.ToTable("Logs");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.UserId)
                  .IsRequired();

        builder.Property(l => l.Method)
               .IsRequired()
               .HasMaxLength(10);

     
        builder.Property(l => l.Route)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(l => l.Body)
               .HasColumnType("TEXT")
               .IsRequired(false);


        builder.Property(l => l.Status)
               .IsRequired()
               .HasMaxLength(20);



    }
}
