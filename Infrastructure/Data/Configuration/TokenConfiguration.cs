using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration;

public class TokenConfiguration : IEntityTypeConfiguration<Token>
{
    public void Configure(EntityTypeBuilder<Token> builder)
    {

        builder.ToTable("Tokens");

       
        builder.HasKey(t => t.Id);

        builder.Property(l => l.UserId)
                 .IsRequired();

    
        builder.Property(t => t.DeviceDetails)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(t => t.SecretKey)
               .IsRequired()
               .HasMaxLength(512);

      
        builder.Property(t => t.ExpiryDate)
               .IsRequired();

      
        builder.Property(t => t.TokenType)
               .HasConversion<string>()
               .IsRequired();

      
        builder.HasCheckConstraint("CHK_TokenType", "TokenType IN ('Access', 'Refresh', 'Verification')");




















    }
}









