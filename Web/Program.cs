using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Web;
using Infrastructure.Settings;

using Business.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.None);
builder.Logging.SetMinimumLevel(LogLevel.Information);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
// Add services to the container.
builder.Services.AddControllersWithViews();

// Register repositories and unit of work
builder.Services.AddRepositories();
// Register services that use unit of work

MapperDi.AddMapper(builder.Services);

ServicesDI.AddServices(builder.Services);
RepositoryServiceExtensions.AddRepositories(builder.Services);
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



// This shouldn't execute - App.Run() should be the last statement
app.MapControllerRoute(
    name: "admin_area",
    pattern: "{area=Admin}/{controller=User}/{action=Edit}/{id?}");
