
using Infrastructure.Data.Repositories;

namespace Infrastructure.Data;

public static class RepositoryServiceExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        // Register repositories
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        services.AddScoped<IOrderTimeLineRepository, OrderTimeLineRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();

        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();




    }
}
