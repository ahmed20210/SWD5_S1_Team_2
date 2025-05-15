using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Web;
using Infrastructure.Settings;
using Business.Services.StorageService;
using Business.Services.OrderService;
using Business.Services.PaymentService;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Business.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.None);
builder.Logging.SetMinimumLevel(LogLevel.Warning);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
// Add services to the container.
builder.Services.AddControllersWithViews();



MapperDi.AddMapper(builder.Services);

ServicesDI.AddServices(builder.Services);
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));




var logger = builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// Configure product images from category folders if needed
if (app.Configuration.GetValue<bool>("UpdateProductImagesOnStartup", false))
{
    await UpdateProductImagesFromCategoryFolders(app);
}

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");


app.UseAuthorization();

app.MapStaticAssets();


app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

// Helper method to update product images from category folders
async Task UpdateProductImagesFromCategoryFolders(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var imageUpdateService = scope.ServiceProvider.GetRequiredService<IProductImageUpdateService>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    try
    {
        logger.LogInformation("Starting product image update from category folders");
        var result = await imageUpdateService.UpdateProductImagesByCategoryAsync();
        logger.LogInformation("Product image update completed. Total: {Total}, Updated: {Updated}", 
            result.totalProducts, result.updatedProducts);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error updating product images from category folders");
    }
}

// This shouldn't execute - App.Run() should be the last statement
app.MapControllerRoute(
    name: "admin_area",
    pattern: "{area=Admin}/{controller=User}/{action=Edit}/{id?}");
