using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configuration;

public class FavouriteListConfigurations : IEntityTypeConfiguration<FavouriteList>
{
    public void Configure(EntityTypeBuilder<FavouriteList> builder)
    {
        builder.ToTable("Favourite List");

        builder.HasKey(i => i.Id);



        builder.Property(i => i.CreatedAt)
             .IsRequired().HasDefaultValueSql("GETDATE()");




    }
}