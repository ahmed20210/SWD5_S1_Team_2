using System.Threading.Tasks;

namespace Business.Services.ProductService
{
    public interface IProductImageUpdateService
    {
        /// <summary>
        /// Updates product image URLs based on images found in the category folders
        /// </summary>
        /// <returns>A summary of how many products were updated</returns>
        Task<(int totalProducts, int updatedProducts)> UpdateProductImagesByCategoryAsync();
        
        /// <summary>
        /// Updates product image URLs for a specific category
        /// </summary>
        /// <param name="categoryName">The name of the category to update products for</param>
        /// <returns>A summary of how many products were updated for this category</returns>
        Task<(int totalProducts, int updatedProducts)> UpdateProductImagesByCategoryAsync(string categoryName);
    }
}
