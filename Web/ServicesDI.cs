namespace Web;
using Business.Services.JwtService;
// using Business.Services.OtpService;
using Business.Services.StorageService;
using Business.Services.MailingService;
using Business.Services.DiscountService;
using Business.Services.AccountService;
// using Business.Services.ProductService;

public static class ServicesDI
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IJwtService, JwtService>();
        // services.AddScoped<IOtpService, OtpService>();
        services.AddScoped<IStorageService, StorageService>();
        services.AddScoped<IMailingService, MailingService>();
        services.AddScoped<IDiscountService, DiscountService>();
        services.AddScoped<IAccountService, AccountService>();
        // services.AddScoped<IProductService, ProductService>();
    }
}
