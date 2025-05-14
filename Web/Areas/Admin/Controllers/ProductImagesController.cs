using System;
using System.IO;
using System.Threading.Tasks;
using Business.Services.ProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    // [Authorize(Roles = "Admin")]
    public class ProductImagesController : Controller
    {
        private readonly IProductImageUpdateService _productImageUpdateService;
        private readonly ILogger<ProductImagesController> _logger;
        private readonly string _outputFilePath;

        public ProductImagesController(
            IProductImageUpdateService productImageUpdateService,
            ILogger<ProductImagesController> logger)
        {
            _productImageUpdateService = productImageUpdateService ?? throw new ArgumentNullException(nameof(productImageUpdateService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            // Set the path to the output file
            string projectRoot = AppDomain.CurrentDomain.BaseDirectory;
            _outputFilePath = Path.Combine(Directory.GetParent(projectRoot).Parent.Parent.FullName, "ecommerce-photo-urls.txt");
        }

        public IActionResult Index()
        {
            ViewBag.UrlFileExists = System.IO.File.Exists(_outputFilePath);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAllCategories()
        {
            try
            {
                var result = await _productImageUpdateService.UpdateProductImagesByCategoryAsync();
                TempData["SuccessMessage"] = $"Product images updated successfully. Total products: {result.totalProducts}, Updated: {result.updatedProducts}";
                TempData["UrlFileGenerated"] = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product images");
                TempData["ErrorMessage"] = $"Error updating product images: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCategory(string categoryName)
        {
            try
            {
                if (string.IsNullOrEmpty(categoryName))
                {
                    TempData["ErrorMessage"] = "Category name is required";
                    return RedirectToAction(nameof(Index));
                }

                var result = await _productImageUpdateService.UpdateProductImagesByCategoryAsync(categoryName);
                TempData["SuccessMessage"] = $"Product images updated for category '{categoryName}'. Total products: {result.totalProducts}, Updated: {result.updatedProducts}";
                TempData["UrlFileGenerated"] = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product images for category {CategoryName}", categoryName);
                TempData["ErrorMessage"] = $"Error updating product images for category '{categoryName}': {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet]
        public IActionResult DownloadUrlFile()
        {
            try
            {
                if (!System.IO.File.Exists(_outputFilePath))
                {
                    TempData["ErrorMessage"] = "URL file does not exist. Please update product images first.";
                    return RedirectToAction(nameof(Index));
                }
                
                return PhysicalFile(_outputFilePath, "text/plain", "ecommerce-photo-urls.txt");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading URL file");
                TempData["ErrorMessage"] = $"Error downloading URL file: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
