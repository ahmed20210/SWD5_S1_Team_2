using Business.Services.CategoryService;
using Business.Services.ProductService;
using Business.ViewModels.ProductViewModels;
using Business.Services.ReviewsService;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Web.Helpers;
using Business.Services.FavouriteListService;
using System.Security.Claims;

namespace Web.Controllers;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IFavouriteListService _favouriteListService;
    private readonly IReviewService _reviewService;
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 50;

    public ProductController(
        ApplicationDbContext db, 
        IProductService productService, 
        ICategoryService categoryService, 
        IFavouriteListService favouriteListService,
        IReviewService reviewService)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        _favouriteListService = favouriteListService ?? throw new ArgumentNullException(nameof(favouriteListService));
        _reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
    }

    public async Task<IActionResult> Index(
        string searchTerm = "",
        int? categoryId = null,
        OrderBy orderBy = OrderBy.NameAsc,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int pageNumber = 1,
        int pageSize = DefaultPageSize,
        string status = null)
    {
        var result = await _productService.GetProductsAsync(
            searchTerm, categoryId, orderBy, minPrice, maxPrice, pageNumber, pageSize, status,
            filterBy: FilterBy.Featured);

        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return View(new List<ProductViewModel>());
        }

        SetViewBag.SetViewBagData(
            this,
            searchTerm,
            categoryId,
            orderBy,
            minPrice,
            maxPrice,
            status,
            result.TotalCount,
            result.TotalPages,
            pageNumber
        );

        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();

        // Get user favorites if user is authenticated
        if (User.Identity.IsAuthenticated)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favorites = await _favouriteListService.GetUserFavouritesAsync(userId);
            ViewBag.UserFavorites = favorites?.Select(p => p.Id).ToList();
        }

        return View(result.Data);
    }

    public async Task<IActionResult> HotDeals(
        string searchTerm = "",
        int? categoryId = null,
        OrderBy orderBy = OrderBy.NameAsc,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int pageNumber = 1,
        int pageSize = DefaultPageSize,
        string status = null
        )
    {
        var result = await _productService.GetProductsAsync(
            searchTerm, categoryId, orderBy, minPrice, maxPrice, pageNumber, pageSize, status,
            filterBy: FilterBy.Discounted);
            // look for discounted products

            Console.WriteLine(result.Data.Count());

        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return View(new List<ProductViewModel>());
        }

        SetViewBag.SetViewBagData(
            this,
            searchTerm,
            categoryId,
            orderBy,
            minPrice,
            maxPrice,
            status,
            result.TotalCount,
            result.TotalPages,
            pageNumber
        );

        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();

        // Get user favorites if user is authenticated
        if (User.Identity.IsAuthenticated)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favorites = await _favouriteListService.GetUserFavouritesAsync(userId);
            ViewBag.UserFavorites = favorites?.Select(p => p.Id).ToList();
        }

        return View(result.Data);
    }

    public async Task<IActionResult> NewArrivals(
        string searchTerm = "",
        int? categoryId = null,
        OrderBy orderBy = OrderBy.NameAsc,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int pageNumber = 1,
        int pageSize = DefaultPageSize,
        string status = null)
    {
        var result = await _productService.GetProductsAsync(
            searchTerm, categoryId, orderBy, minPrice, maxPrice, pageNumber, pageSize, status,
            filterBy: FilterBy.NewArrivals);

        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return View(new List<ProductViewModel>());
        }

        SetViewBag.SetViewBagData(
            this,
            searchTerm,
            categoryId,
            orderBy,
            minPrice,
            maxPrice,
            status,
            result.TotalCount,
            result.TotalPages,
            pageNumber
        );

        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();

        // Get user favorites if user is authenticated
        if (User.Identity.IsAuthenticated)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favorites = await _favouriteListService.GetUserFavouritesAsync(userId);
            ViewBag.UserFavorites = favorites?.Select(p => p.Id).ToList();
        }

        return View(result.Data);
    }

    public async Task<IActionResult> BestSellers(
        string searchTerm = "",
        int? categoryId = null,
        OrderBy orderBy = OrderBy.NameAsc,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int pageNumber = 1,
        int pageSize = DefaultPageSize,
        string status = null)
    {
        var result = await _productService.GetProductsAsync(
            searchTerm, categoryId, orderBy, minPrice, maxPrice, pageNumber, pageSize, status,
            filterBy: FilterBy.BestSelling);

        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return View(new List<ProductViewModel>());
        }

        SetViewBag.SetViewBagData(
            this,
            searchTerm,
            categoryId,
            orderBy,
            minPrice,
            maxPrice,
            status,
            result.TotalCount,
            result.TotalPages,
            pageNumber
        );

        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();

        // Get user favorites if user is authenticated
        if (User.Identity.IsAuthenticated)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favorites = await _favouriteListService.GetUserFavouritesAsync(userId);
            ViewBag.UserFavorites = favorites?.Select(p => p.Id).ToList();
        }

        return View(result.Data);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var result = await _productService.GetProductByIdAsync(id);
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction("Index");
        }

        var product = result.Data;
        
        // Increment the view count for this product (handled in a separate task to avoid delaying the response)
        _ = Task.Run(async () => {
            try {
                await _db.Database.ExecuteSqlRawAsync($"UPDATE Products SET NoOfViews = NoOfViews + 1 WHERE Id = {id}");
            } catch {}
        });

        // Check if this product is in the user's favorites
        if (User.Identity.IsAuthenticated)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favorites = await _favouriteListService.GetUserFavouritesAsync(userId);
            ViewBag.IsFavorite = favorites?.Any(p => p.Id == id) ?? false;
            
            // Check if user has already reviewed this product
            var reviewsResult = await _reviewService.GetReviewsAsync(productId: id, userId: userId);
            ViewBag.UserHasReviewed = reviewsResult.Success && reviewsResult.Data.Any();
        }
        
        // Get top 3 reviews for preview
        var topReviewsResult = await _reviewService.GetReviewsAsync(
            productId: id, 
            isVerified: true,
            pageNumber: 1, 
            pageSize: 3);
            
        if (topReviewsResult.Success)
        {
            ViewBag.TopReviews = topReviewsResult.Data;
        }

        return View(product);
    }

    public async Task<IActionResult> Category(
        int id,
         string searchTerm = "",
        OrderBy orderBy = OrderBy.NameAsc,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int pageNumber = 1,
        int pageSize = DefaultPageSize,
        string status = null)
    {
        var result = await _productService.GetProductsAsync(
            searchTerm, id, orderBy, minPrice, maxPrice, pageNumber, pageSize, status);

        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return View(new List<ProductViewModel>());
        }

        // Get category name
        var categoryResult = await _categoryService.GetCategoryByIdAsync(id);
        
            ViewBag.CategoryName = categoryResult?.Name;
        

        SetViewBag.SetViewBagData(
            this,
            searchTerm,
            id,
            orderBy,
            minPrice,
            maxPrice,
            status,
            result.TotalCount,
            result.TotalPages,
            pageNumber
        );

        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();

        // Get user favorites if user is authenticated
        if (User.Identity.IsAuthenticated)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favorites = await _favouriteListService.GetUserFavouritesAsync(userId);
            ViewBag.UserFavorites = favorites?.Select(p => p.Id).ToList();
        }

        return View(result.Data);
    }
}
