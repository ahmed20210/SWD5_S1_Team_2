using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

using Infrastructure.Data.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace Infrastructure.Data;

public partial class ApplicationDbContext : IdentityDbContext<User>
{
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

        EntityConfigurations.ApplyConfigurations(modelBuilder);
    
  
    
    }
  
}
