using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configuration;

public class OrderTimeLineConfigurations : IEntityTypeConfiguration<OrderTimeLine>
{
    public void Configure(EntityTypeBuilder<OrderTimeLine> builder)
    {
        builder.ToTable("Order TimeLine");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Status)
            .IsRequired();

        builder.Property(i => i.Description)
            .HasMaxLength(100).HasDefaultValue("No Description Available.");

        builder.Property(i => i.ChangedAt)
           .IsRequired().HasDefaultValueSql("GETDATE()");





    }
}
