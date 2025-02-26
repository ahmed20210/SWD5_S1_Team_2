using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration;

public class NotificationConfiguration :IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Message)
            .IsRequired().HasColumnType("nvarchar(200)");
        builder.Property(x => x.CreatedAt)
            .HasColumnType("datetime2").HasDefaultValueSql("getdate()");
        builder.Property(x => x.IsRead).HasDefaultValue(false);
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.Title).IsRequired().HasColumnType("nvarchar(50)");
        builder.Property(x => x.URL).HasColumnType("nvarchar(100)");
        builder.Property(x => x.ReadAt).HasColumnType("datetime2");
        // builder.HasIndex(x => x.UserId);
    }
    
}