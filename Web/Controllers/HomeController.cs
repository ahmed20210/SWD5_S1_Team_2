using System.Diagnostics;
using Business.Services.CategoryService;
using Business.Services.FavouriteListService;
using Business.Services.ProductService;
using Business.ViewModels.ProductViewModels;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Web.Models;

namespace Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IFavouriteListService _favouriteListService;

    public HomeController(
        ILogger<HomeController> logger,
        IProductService productService,
        ICategoryService categoryService,
        IFavouriteListService favouriteListService)
    {
        _categoryService = categoryService;
        _productService = productService;
        _favouriteListService = favouriteListService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var NewArrivals = await _productService.GetProductsAsync(
               filterBy: FilterBy.NewArrivals,
               pageNumber: 1,
               pageSize: 12
           );

            var HotDeals = await _productService.GetProductsAsync(
                filterBy: FilterBy.Discounted,
                pageNumber: 1,
                pageSize: 12
            );

            var BestSelling = await _productService.GetProductsAsync(
                filterBy: FilterBy.BestSelling,
                pageNumber: 1,
                pageSize: 12
            );

            var Categories = await _categoryService.GetAllCategoriesAsync();
            
            if (!NewArrivals.Success || !HotDeals.Success)
            {
                // Handle error - you might want to log this or show an error message
                TempData["Error"] = "An error occurred while fetching data.";
                return View();
            }

            // Get user favorites if authenticated
            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(userId))
                {
                    var favorites = await _favouriteListService.GetUserFavouritesAsync(userId);
                    ViewBag.UserFavorites = favorites?.Select(p => p.Id).ToList();
                }
            }


            return View(new HomeViewModel(
                NewArrivals: NewArrivals.Data ?? new List<ProductViewModel>(),
                HotDeals: HotDeals.Data ?? new List<ProductViewModel>(),
                BestSelling: BestSelling.Data ?? new List<ProductViewModel>(),
                Categories: Categories
            ));
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching data.");
            TempData["Error"] = "An error occurred while fetching data.";
            return View();
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
