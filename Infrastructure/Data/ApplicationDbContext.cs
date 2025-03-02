using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

using Infrastructure.Data.Configuration;

namespace Infrastructure.Data;

public partial class ApplicationDbContext : DbContext
{
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    { 
   
    EntityConfigurations.ApplyConfigurations(modelBuilder);
    
  
    
    }
  
}