using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

using Infrastructure.Data.Configuration;

namespace Infrastructure.Data;

public partial class ApplicationDbContext : DbContext
{
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    { 
   
    EntityConfigurations.ApplyConfigurations(modelBuilder);
    
  
    
    }
  
}