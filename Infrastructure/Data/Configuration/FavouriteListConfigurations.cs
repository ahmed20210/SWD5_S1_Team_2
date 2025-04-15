using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configuration;

public class FavouriteListConfigurations : IEntityTypeConfiguration<FavouriteList>
{
    public void Configure(EntityTypeBuilder<FavouriteList> builder)
    {
        builder.HasKey(f => new { f.UserId, f.ProductId });
        builder.HasOne(f => f.Product)
            .WithMany()
            .HasForeignKey(f => f.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        
    }
}