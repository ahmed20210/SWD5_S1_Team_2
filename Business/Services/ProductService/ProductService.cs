using AutoMapper;
using AutoMapper.QueryableExtensions;
using Business.ViewModels.ProductViewModels;
using Domain;
using Domain.Response;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Business.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private const int DefaultPageSize = 10;
        private const int MaxPageSize = 50;

        public ProductService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GenericResponse<IEnumerable<ProductViewModel>>> GetProductsAsync(
            string searchTerm = "",
            int? categoryId = null,
            OrderBy orderBy = OrderBy.NameAsc,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            int pageNumber = 1,
            int pageSize = DefaultPageSize,
            string? status = null
            )
        {

            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            pageSize = Math.Clamp(pageSize, 1, MaxPageSize);

            var query = _db.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(p =>
                    p.Name.ToLower().Contains(searchTerm) ||
                    (p.Description != null && p.Description.ToLower().Contains(searchTerm)));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.Category.Id == categoryId.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            // Apply sorting
            query = orderBy switch
            {
                OrderBy.NameAsc => query.OrderBy(p => p.Name),
                OrderBy.NameDesc => query.OrderByDescending(p => p.Name),
                OrderBy.PriceAsc => query.OrderBy(p => p.Price),
                OrderBy.PriceDesc => query.OrderByDescending(p => p.Price),
                OrderBy.NewestAsc => query.OrderBy(p => p.CreatedAt),
                OrderBy.NewestDesc => query.OrderByDescending(p => p.CreatedAt),
                OrderBy.PopularityAsc => query.OrderBy(p => p.NoOfViews),
                OrderBy.PopularityDesc => query.OrderByDescending(p => p.NoOfViews),
                _ => query.OrderBy(p => p.Name)
            };
            int totalRecords = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            var products = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(p => p.Category)
                .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            
            return GenericResponse<IEnumerable<ProductViewModel>>.SuccessResponse(products, "Products retrieved successfully", totalRecords, pageNumber, totalPages);
        }

    }

    //     public async Task<BaseResponse<Product>> GetProductByIdAsync(int id)
    //     {
    //         try
    //         {
    //             var product = await _db.Products
    //                 .Include(p => p.Category)
    //                 .FirstOrDefaultAsync(p => p.Id == id);

    //             if (product == null)
    //             {
    //                 return BaseResponse<Product>.FailureResponse($"Product with ID {id} not found");
    //             }

    //             return BaseResponse<Product>.SuccessResponse("Product retrieved successfully", product);
    //         }
    //         catch (Exception ex)
    //         {
    //             return BaseResponse<Product>.FailureResponse($"Error retrieving product: {ex.Message}");
    //         }
    //     }

    //     public async Task<BaseResponse> CreateProductAsync(ProductCreateViewModel model)
    //     {
    //         try
    //         {
    //             if (model == null)
    //             {
    //                 return BaseResponse.FailureResponse("Product model is null");
    //             }

    //             var category = await _db.Categories.FindAsync(model.CategoryId);
    //             if (category == null)
    //             {
    //                 return BaseResponse.FailureResponse("Invalid category selected");
    //             }

    //             var product = new Product
    //             {
    //                 Name = model.Name,
    //                 Description = model.Description,
    //                 Price = model.Price,
    //                 Category = category,
    //                 CreatedAt = DateTime.UtcNow,
    //                 NoOfViews = 0,
    //                 Status = ProductStatus.Active
    //             };

    //             _db.Products.Add(product);
    //             await _db.SaveChangesAsync();

    //             return BaseResponse.SuccessResponse("Product created successfully");
    //         }
    //         catch (Exception ex)
    //         {
    //             return BaseResponse.FailureResponse($"Error creating product: {ex.Message}");
    //         }
    //     }

    //     public async Task<BaseResponse<List<Category>>> GetCategoriesAsync()
    //     {
    //         try
    //         {
    //             var categories = await _db.Categories
    //                 .OrderBy(c => c.Name)
    //                 .ToListAsync();

    //             return BaseResponse<List<Category>>.SuccessResponse("Categories retrieved successfully", categories);
    //         }
    //         catch (Exception ex)
    //         {
    //             return BaseResponse<List<Category>>.FailureResponse($"Error retrieving categories: {ex.Message}");
    //         }
    //     }
    // }
}
