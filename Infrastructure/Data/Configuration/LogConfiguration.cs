using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;
using Domain;

namespace Infrastructure.Data.Configuration;

public class LogConfiguration : IEntityTypeConfiguration<Log>
{
    public void Configure(EntityTypeBuilder<Log> builder)
    {
        builder.Property(l => l.Method)
            .IsRequired()
            .HasConversion<string>();
        
        builder.Property(l => l.Status)
            .IsRequired()
            .HasConversion<string>();
        
        builder.HasOne(l => l.User)
            .WithMany()
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}