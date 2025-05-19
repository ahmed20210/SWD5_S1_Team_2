namespace Web;
using Business.Services.JwtService;
using Business.Services.StorageService;
using Business.Services.MailingService;
using Business.Services.DiscountService;
using Business.Services.AccountService;
using Business.Services.CategoryService;
using Business.Services.UserService;
using Business.Services.ProductService;
using Business.Services.PaymentService;
using Business.Services.FavouriteListService;
using Business.Services.OrderService;
using Business.Services.AddressService;
using Business.Services.ReviewsService;
using Business.Services.OrderItemService;
using Business.Services.OrderTimeLineService;

public static class ServicesDI
{
    public static void AddServices(this IServiceCollection services)
    {

        // Register services not using UnitOfWork
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IStorageService, StorageService>();
        services.AddScoped<IMailingService, MailingService>();
        services.AddScoped<IDiscountService, DiscountService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductImageUpdateService, ProductImageUpdateService>();

        services.AddScoped<IPaymentGatewayService, StripePaymentGatewayService>();
        services.AddScoped<IFavouriteListService, FavouriteListService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAddressService, AddressService>();
        services.AddScoped<IReviewService, ReviewService>();

        // Register services that use Unit of Work
        services.AddScoped<IOrderServiceUsingUnitOfWork, OrderServiceUsingUnitOfWork>();
        services.AddScoped<IOrderItemServiceWithUnitOfWork, OrderItemServiceWithUnitOfWork>();
        services.AddScoped<IOrderTimeLineService, OrderTimeLineService>();
        services.AddScoped<IPaymentServiceUsingUnitOfWork, PaymentServiceUsingUnitOfWork>();
    }
}
