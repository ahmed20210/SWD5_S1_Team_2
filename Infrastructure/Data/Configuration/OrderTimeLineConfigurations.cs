using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configuration;

public class OrderTimeLineConfigurations : IEntityTypeConfiguration<OrderTimeLine>
{
    public void Configure(EntityTypeBuilder<OrderTimeLine> builder)
    {
        builder.Property(otl => otl.Status)
            .HasConversion<string>();
    }
}