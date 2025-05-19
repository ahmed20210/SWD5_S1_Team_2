
using Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data;

public static class UnitOfWorkServiceExtensions
{
    public static IServiceCollection AddUnitOfWorkServices(this IServiceCollection services)
    {
        // Register repositories
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        services.AddScoped<IOrderTimeLineRepository, OrderTimeLineRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        
        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
       
        
        return services;
    }
}
