
namespace Web;
using Business.Services.JwtService;
using Business.Services.StorageService;
using Business.Services.MailingService;
using Business.Services.DiscountService;
using Business.Services.AccountService;
using Business.Services.CategoryService;
using Business.Services.UserService;

using Business.Services.ProductService;
using Business.Services.OrderItemService;
using Business.Services.PaymentService;

public static class ServicesDI
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IStorageService, StorageService>();
        services.AddScoped<IMailingService, MailingService>();
        services.AddScoped<IDiscountService, DiscountService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IOrderItemService, OrderItemService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IPaymentGatewayService, StripePaymentGatewayService>();
    }
}
