
using Business.Services.ProductService;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Domain;
using Business.Services.CategoryService;

namespace Web.Areas.Admin.Controllers;

[Area("Admin")]

public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 50;

    public ProductController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
    }

    public async Task<IActionResult> Index(
        string searchTerm = "",
        int? categoryId = null,
        OrderBy orderBy = OrderBy.NameAsc,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int pageNumber = 1,
        int pageSize = DefaultPageSize,
        string? status = null)
    {
        try
        {
            var result = await _productService.GetProductsAsync(
            searchTerm, categoryId, orderBy, minPrice, maxPrice, pageNumber, pageSize, status);
            ViewBag.SearchTerm = searchTerm;
            ViewBag.CategoryId = categoryId;
            ViewBag.OrderBy = orderBy;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.Status = status;
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.TotalCount = result.TotalCount;
            ViewBag.TotalPages = (int)Math.Ceiling((double)result.TotalCount / pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.products = result.Data;


            return View(result.Data);

        }
        catch (Exception ex)
        {
            // Handle error - you might want to log this or show an error message
            TempData["Error"] = ex.Message;
            return View(new List<ProductViewModel>());
        }




    }

    // [HttpGet]
    // public async Task<IActionResult> Create()
    // {
    //     var categoriesResult = await _productService.GetCategoriesAsync();

    //     if (!categoriesResult.IsSuccess)
    //     {
    //         TempData["Error"] = categoriesResult.ErrorMessage;
    //         return View(new ProductCreateViewModel());
    //     }

    //     var model = new ProductCreateViewModel
    //     {
    //         AvailableCategories = categoriesResult.Data
    //     };
    //     return View(model);
    // }

    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> Create(ProductCreateViewModel model)
    //     {
    //         if (!ModelState.IsValid)
    //         {
    //             var categoriesResult = await _productService.GetCategoriesAsync();
    //     model.AvailableCategories = categoriesResult.IsSuccess? categoriesResult.Data : new List<Category>();
    //             return View(model);
    //         }

    //         var result = await _productService.CreateProductAsync(model);

    //         if (!result.IsSuccess)
    //         {
    //             TempData["Error"] = result.ErrorMessage;
    //             var categoriesResult = await _productService.GetCategoriesAsync();
    // model.AvailableCategories = categoriesResult.IsSuccess? categoriesResult.Data : new List<Category>();
    //             return View(model);
    //         }

    //         return RedirectToAction(nameof(Index));
    //     }


}
/*

*/
