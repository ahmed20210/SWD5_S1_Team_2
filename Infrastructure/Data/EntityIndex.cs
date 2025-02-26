
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public partial  class ApplicationDbContext
{
   public DbSet<Payment> Payments { get; set; }
   public DbSet<Notification> Notifications { get; set; }
   public DbSet<ReturnedOrder> ReturnedOrders { get; set; }
}
