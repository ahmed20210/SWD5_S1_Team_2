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
            string? status = null,
            bool includeOutOfStock = false,
            FilterBy filterBy = FilterBy.Featured
            );
            
        Task<GenericResponse<ProductViewModel>> GetProductByIdAsync(int id);

        Task<GenericResponse<ProductViewModel>> CreateProductAsync(CreateProductViewModel model);

        Task<GenericResponse<ProductViewModel>> UpdateProductAsync(UpdateProductViewModel model);

        Task<bool> DeleteProductAsync(int id);
        // Task<BaseResponse<Product>> GetProductByIdAsync(int id);
        
        // Task<BaseResponse> CreateProductAsync(ProductCreateViewModel model);
        
        // Task<BaseResponse<List<Category>>> GetCategoriesAsync();
    }
}
