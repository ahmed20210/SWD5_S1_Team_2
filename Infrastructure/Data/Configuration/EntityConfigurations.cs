using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Configuration;

public static class EntityConfigurations
{
    public static void ApplyConfigurations(ModelBuilder modelBuilder)
    {
        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(EntityConfigurations).Assembly);

        modelBuilder.ApplyConfiguration(new NotificationConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentConfiguration());
        modelBuilder.ApplyConfiguration(new ReturnedOrderConfiguration());
        modelBuilder.ApplyConfiguration(new DiscountConfigurations());
        modelBuilder.ApplyConfiguration(new OrderConfigurations());
        modelBuilder.ApplyConfiguration(new OrderItemConfigurations());
    }

}