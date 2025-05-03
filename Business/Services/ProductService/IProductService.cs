// using Domain.DTOs.ProductDTOs;
// using Domain.Entities;
// using Domain.Response;

// namespace Business.Services.ProductService
// {
//     public interface IProductService
//     {
//         Task<BaseResponse<ProductViewModel>> GetProductsAsync(
//             string searchTerm = "",
//             int? categoryId = null,
//             string orderBy = "name",
//             decimal? minPrice = null,
//             decimal? maxPrice = null,
//             int pageNumber = 1,
//             int pageSize = 10);
            
//         Task<BaseResponse<Product>> GetProductByIdAsync(int id);
        
//         Task<BaseResponse> CreateProductAsync(ProductCreateViewModel model);
        
//         Task<BaseResponse<List<Category>>> GetCategoriesAsync();
//     }
// }
