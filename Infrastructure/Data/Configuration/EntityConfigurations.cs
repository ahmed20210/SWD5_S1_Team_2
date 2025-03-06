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
        modelBuilder.ApplyConfiguration(new LogConfiguration());
        modelBuilder.ApplyConfiguration(new CouponConfiguration());
        modelBuilder.ApplyConfiguration(new TokenConfiguration());

        modelBuilder.ApplyConfiguration(new ProductConfigurations());
        modelBuilder.ApplyConfiguration(new ProductImageConfigurations());
        modelBuilder.ApplyConfiguration(new ReviewConfigurations());
        modelBuilder.ApplyConfiguration(new UserConfigurations());
        modelBuilder.ApplyConfiguration(new AddressConfigurations());
        modelBuilder.ApplyConfiguration(new CategoryConfigurations());

    }

}