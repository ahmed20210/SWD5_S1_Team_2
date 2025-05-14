using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Business.Services.StorageService;
using System.Text;

namespace Business.Services.ProductService
{
    public class ProductImageUpdateService : IProductImageUpdateService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ProductImageUpdateService> _logger;
        private readonly string _ecommerceFolderPath;
        private readonly IStorageService _storageService;
        private readonly string _outputFilePath;
        
        public ProductImageUpdateService(
            ApplicationDbContext dbContext,
            ILogger<ProductImageUpdateService> logger,
            IStorageService storageService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
            
            // Set the fixed path to the Ecommerce folder
            _ecommerceFolderPath = "/home/sharabash/RiderProjects/TechXpress/Business/Ecommerce";
            
            // Use consistent file name - match the one defined in Web project
            _outputFilePath = "/home/sharabash/RiderProjects/TechXpress/Web/ecommerce-photo-links.txt";
            
            if (!Directory.Exists(_ecommerceFolderPath))
            {
                _logger.LogWarning("Ecommerce folder path not found: {Path}", _ecommerceFolderPath);
            }
            
            _logger.LogInformation("Ecommerce folder path: {Path}", _ecommerceFolderPath);
            _logger.LogInformation("Output file path: {Path}", _outputFilePath);
        }
        
        /// <inheritdoc />
        public async Task<(int totalProducts, int updatedProducts)> UpdateProductImagesByCategoryAsync()
        {
            int totalProductsUpdated = 0;
            int totalProductsCount = 0;
            
            // Ensure directory exists for the output file
            Directory.CreateDirectory(Path.GetDirectoryName(_outputFilePath));
            
            // Get all categories
            var categories = await _dbContext.Categories.ToListAsync();
            _logger.LogInformation("Processing {Count} categories", categories.Count);
            
            if (categories.Count == 0)
            {
                _logger.LogWarning("No categories found in the database");
                return (0, 0);
            }
            
            // Create or clear the output file
            await File.WriteAllTextAsync(_outputFilePath, $"Product Image URLs - Generated on {DateTime.Now}\n\n");
            
            foreach (var category in categories)
            {
                var result = await UpdateProductImagesByCategoryAsync(category.Name);
                totalProductsCount += result.totalProducts;
                totalProductsUpdated += result.updatedProducts;
            }
            
            if (totalProductsUpdated > 0)
            {
                _logger.LogInformation("Successfully updated {Count} product images across all categories", 
                    totalProductsUpdated);
            }
            else
            {
                _logger.LogWarning("No product images were updated");
            }
            
            return (totalProductsCount, totalProductsUpdated);
        }
        
        /// <inheritdoc />
        public async Task<(int totalProducts, int updatedProducts)> UpdateProductImagesByCategoryAsync(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                throw new ArgumentNullException(nameof(categoryName));
            }
            
            _logger.LogInformation("Processing category: {CategoryName}", categoryName);
            
            // Find the category by name - fixed to use ToLower() on both sides instead of Equals with StringComparison
            var category = await _dbContext.Categories
                .Where(c => c.Name.ToLower() == categoryName.ToLower())
                .FirstOrDefaultAsync();
            
            if (category == null)
            {
                _logger.LogWarning("Category not found: {CategoryName}", categoryName);
                return (0, 0);
            }
            
            // Get products for this category
            var products = await _dbContext.Products
                .Where(p => p.CategoryId == category.Id)
                .ToListAsync();
            
            if (!products.Any())
            {
                _logger.LogInformation("No products found for category: {CategoryName}", categoryName);
                return (0, 0);
            }
            
            // Find the category folder - ensure proper case sensitivity match
            string categoryFolderPath = null;
            
            // Check if direct match exists
            if (Directory.Exists(Path.Combine(_ecommerceFolderPath, categoryName)))
            {
                categoryFolderPath = Path.Combine(_ecommerceFolderPath, categoryName);
            }
            else
            {
                // Try to find a case-insensitive match
                var allDirs = Directory.GetDirectories(_ecommerceFolderPath);
                foreach (var dir in allDirs)
                {
                    if (Path.GetFileName(dir).Equals(categoryName, StringComparison.OrdinalIgnoreCase))
                    {
                        categoryFolderPath = dir;
                        break;
                    }
                }
            }
            
            if (categoryFolderPath == null)
            {
                _logger.LogWarning("Category folder not found: {CategoryName} at path {Path}", 
                    categoryName, _ecommerceFolderPath);
                return (products.Count, 0);
            }
            
            // Get all image files from the category folder
            var imageFiles = Directory.GetFiles(categoryFolderPath, "*.*", SearchOption.AllDirectories)
                .Where(file => IsImageFile(file))
                .ToList();
            
            if (!imageFiles.Any())
            {
                _logger.LogWarning("No image files found in category folder: {Path}", categoryFolderPath);
                return (products.Count, 0);
            }
            
            _logger.LogInformation("Found {Count} images for category {CategoryName}", 
                imageFiles.Count, categoryName);
            
            // Append category section to URL file
            await AppendToUrlFile($"\n== Category: {categoryName} ==\n");
            
            // Upload images and update products
            int productsUpdated = await UploadImagesAndUpdateProducts(products, imageFiles, categoryName);
            
            return (products.Count, productsUpdated);
        }
        
        private async Task<int> UploadImagesAndUpdateProducts(List<Product> products, List<string> imageFiles, string categoryName)
        {
            int updatedCount = 0;
            StringBuilder urlMapping = new StringBuilder();
            
            // Process images and products in order
            for (int i = 0; i < Math.Min(products.Count, imageFiles.Count); i++)
            {
                var product = products[i];
                string imagePath = imageFiles[i];
                string fileName = Path.GetFileName(imagePath);
                string contentType = GetContentType(imagePath);
                
                try
                {
                    _logger.LogInformation("Processing image {ImagePath} for product {ProductName} (ID: {ProductId})", 
                        imagePath, product.Name, product.Id);
                        
                    // Open file stream for upload
                    using var fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                    
                    // Upload image using StorageService
                    var uploadResult = await _storageService.UploadPhotoAsync(fileStream, fileName, contentType);
                    
                    if (uploadResult.success)
                    {
                        // Update product with new image URL
                        product.ImageUrl = uploadResult.fileUrl;
                        
                        // Log the mapping
                        string mapping = $"Product: {product.Name} (ID: {product.Id})\nImage: {fileName}\nURL: {uploadResult.fileUrl}\n";
                        urlMapping.AppendLine(mapping);
                        
                        updatedCount++;
                        _logger.LogInformation("Uploaded image for product {ProductId} ({ProductName}): {ImageUrl}", 
                            product.Id, product.Name, uploadResult.fileUrl);
                    }
                    else
                    {
                        _logger.LogWarning("Failed to upload image {FileName} for product {ProductId}: {ErrorMessage}", 
                            fileName, product.Id, uploadResult.message);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing image {ImagePath} for product {ProductId}", 
                        imagePath, product.Id);
                }
            }
            
            if (updatedCount > 0)
            {
                // Save changes to database
                await _dbContext.SaveChangesAsync();
                
                // Append mappings to the URL file
                await AppendToUrlFile(urlMapping.ToString());
                
                _logger.LogInformation("Saved {Count} product image updates for category {CategoryName}", 
                    updatedCount, categoryName);
            }
            
            return updatedCount;
        }
        
        private async Task AppendToUrlFile(string content)
        {
            try
            {
                await File.AppendAllTextAsync(_outputFilePath, content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to append to URL file: {FilePath}", _outputFilePath);
            }
        }
        
        private bool IsImageFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension == ".jpg" || extension == ".jpeg" || extension == ".png" || 
                   extension == ".gif" || extension == ".webp" || extension == ".bmp";
        }
        
        private string GetContentType(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                ".bmp" => "image/bmp",
                _ => "application/octet-stream"
            };
        }
    }
}
