// using AutoMapper;
// using Domain.DTOs.ProductDTOs;
// using Domain.Entities;
// using Domain.Response;
// using Infrastructure.Data;
// using Microsoft.EntityFrameworkCore;
// using Web.Models;

// namespace Business.Services.ProductService
// {
//     public class ProductService : IProductService
//     {
//         private readonly ApplicationDbContext _db;
//         private readonly IMapper _mapper;
//         private const int DefaultPageSize = 10;
//         private const int MaxPageSize = 50;

//         public ProductService(ApplicationDbContext db, IMapper mapper)
//         {
//             _db = db ?? throw new ArgumentNullException(nameof(db));
//             _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
//         }

//         public async Task<BaseResponse<ProductBaseViewModel>> GetProductsAsync(
//             string searchTerm = "",
//             int? categoryId = null,
//             string orderBy = "name",
//             decimal? minPrice = null,
//             decimal? maxPrice = null,
//             int pageNumber = 1,
//             int pageSize = DefaultPageSize)
//         {
//             try
//             {
//                 // Validate and adjust page size
//                 pageSize = Math.Clamp(pageSize, 1, MaxPageSize);

//                 // Base query with include for Category
//                 var query = _db.Products.Include(p => p.Category).AsQueryable();

//                 // Apply search filter
//                 if (!string.IsNullOrWhiteSpace(searchTerm))
//                 {
//                     searchTerm = searchTerm.ToLower();
//                     query = query.Where(p =>
//                         p.Name.ToLower().Contains(searchTerm) ||
//                         (p.Description != null && p.Description.ToLower().Contains(searchTerm)));
//                 }

//                 // Apply category filter
//                 if (categoryId.HasValue)
//                 {
//                     query = query.Where(p => p.Category.Id == categoryId.Value);
//                 }

//                 // Apply price range filter
//                 if (minPrice.HasValue)
//                 {
//                     query = query.Where(p => p.Price >= minPrice.Value);
//                 }

//                 if (maxPrice.HasValue)
//                 {
//                     query = query.Where(p => p.Price <= maxPrice.Value);
//                 }

//                 // Apply sorting
//                 query = orderBy.ToLower() switch
//                 {
//                     "price_desc" => query.OrderByDescending(p => p.Price),
//                     "price" => query.OrderBy(p => p.Price),
//                     "created_desc" => query.OrderByDescending(p => p.CreatedAt),
//                     "created" => query.OrderBy(p => p.CreatedAt),
//                     "popularity_desc" => query.OrderByDescending(p => p.NoOfViews),
//                     "popularity" => query.OrderBy(p => p.NoOfViews),
//                     "category" => query.OrderBy(p => p.Category.Name),
//                     "category_desc" => query.OrderByDescending(p => p.Category.Name),
//                     _ => query.OrderBy(p => p.Name)
//                 };

//                 // Get total count before pagination
//                 int totalRecords = await query.CountAsync();
//                 int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

//                 // Apply pagination
//                 var products = await query
//                     .Skip((pageNumber - 1) * pageSize)
//                     .Take(pageSize)
//                     .Select(p => new Product
//                     {
//                         Id = p.Id,
//                         Name = p.Name,
//                         Description = p.Description,
//                         Price = p.Price,
//                         Category = p.Category,
//                         CreatedAt = p.CreatedAt,
//                         NoOfViews = p.NoOfViews
//                     })
//                     .ToListAsync();

//                 // Get available categories for filter dropdown
//                 var availableCategories = await _db.Categories
//                     .OrderBy(c => c.Name)
//                     .ToListAsync();

//                 // Prepare view model
//                 var model = new ProductViewModel
//                 {
//                     Products = products,
//                     CurrentPage = pageNumber,
//                     TotalPages = totalPages,
//                     PageSize = pageSize,
//                     SearchTerm = searchTerm,
//                     CategoryId = categoryId,
//                     OrderBy = orderBy,
//                     MinPrice = minPrice,
//                     MaxPrice = maxPrice,
//                     AvailableCategories = availableCategories,
//                     TotalCount = totalRecords
//                 };

//                 return BaseResponse<ProductViewModel>.SuccessResponse("Products retrieved successfully", model);
//             }
//             catch (Exception ex)
//             {
//                 return BaseResponse<ProductViewModel>.FailureResponse($"Error retrieving products: {ex.Message}");
//             }
//         }

//         public async Task<BaseResponse<Product>> GetProductByIdAsync(int id)
//         {
//             try
//             {
//                 var product = await _db.Products
//                     .Include(p => p.Category)
//                     .FirstOrDefaultAsync(p => p.Id == id);

//                 if (product == null)
//                 {
//                     return BaseResponse<Product>.FailureResponse($"Product with ID {id} not found");
//                 }

//                 return BaseResponse<Product>.SuccessResponse("Product retrieved successfully", product);
//             }
//             catch (Exception ex)
//             {
//                 return BaseResponse<Product>.FailureResponse($"Error retrieving product: {ex.Message}");
//             }
//         }

//         public async Task<BaseResponse> CreateProductAsync(ProductCreateViewModel model)
//         {
//             try
//             {
//                 if (model == null)
//                 {
//                     return BaseResponse.FailureResponse("Product model is null");
//                 }

//                 var category = await _db.Categories.FindAsync(model.CategoryId);
//                 if (category == null)
//                 {
//                     return BaseResponse.FailureResponse("Invalid category selected");
//                 }

//                 var product = new Product
//                 {
//                     Name = model.Name,
//                     Description = model.Description,
//                     Price = model.Price,
//                     Category = category,
//                     CreatedAt = DateTime.UtcNow,
//                     NoOfViews = 0,
//                     Status = ProductStatus.Active
//                 };

//                 _db.Products.Add(product);
//                 await _db.SaveChangesAsync();

//                 return BaseResponse.SuccessResponse("Product created successfully");
//             }
//             catch (Exception ex)
//             {
//                 return BaseResponse.FailureResponse($"Error creating product: {ex.Message}");
//             }
//         }

//         public async Task<BaseResponse<List<Category>>> GetCategoriesAsync()
//         {
//             try
//             {
//                 var categories = await _db.Categories
//                     .OrderBy(c => c.Name)
//                     .ToListAsync();

//                 return BaseResponse<List<Category>>.SuccessResponse("Categories retrieved successfully", categories);
//             }
//             catch (Exception ex)
//             {
//                 return BaseResponse<List<Category>>.FailureResponse($"Error retrieving categories: {ex.Message}");
//             }
//         }
//     }
// }
