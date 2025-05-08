using Business.ViewModels.ProductViewModels;
using Domain;
using Domain.Response;

namespace Business.Services.ProductService
{
    public interface IProductService
    {
        Task<GenericResponse<IEnumerable<ProductViewModel>>> GetProductsAsync(
            string searchTerm = "",
            int? categoryId = null,
            OrderBy orderBy = OrderBy.NameAsc,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            int pageNumber = 1,
            int pageSize = 10,
            string? status = null
            );
            
        // Task<BaseResponse<Product>> GetProductByIdAsync(int id);
        
        // Task<BaseResponse> CreateProductAsync(ProductCreateViewModel model);
        
        // Task<BaseResponse<List<Category>>> GetCategoriesAsync();
    }
}
