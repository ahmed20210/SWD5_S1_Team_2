
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public partial  class ApplicationDbContext
{
   public DbSet<Payment> Payments { get; set; }
   public DbSet<Notification> Notifications { get; set; }
   public DbSet<ReturnedOrder> ReturnedOrders { get; set; }
   public DbSet<Order> Orders { get; set; }
   public DbSet<OrderItem> OrderItems { get; set; }
   public DbSet<Discount> Discounts{ get; set; }
   public DbSet<Product> Products{ get; set; }
   public DbSet<ProductImage> ProductImages{ get; set; }
   public DbSet<Review> Reviews { get; set; }

}
