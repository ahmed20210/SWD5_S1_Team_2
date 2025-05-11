using AutoMapper;
using AutoMapper.QueryableExtensions;
using Business.Services.StorageService;
using Business.ViewModels.ProductViewModels;
using Domain;
using Domain.Entities;
using Domain.Response;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Business.Services.ProductService;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _db;
    private readonly IStorageService _storageService;
    private readonly IMapper _mapper;
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 50;

    private const int NewProductsDays = 30;

    public ProductService(ApplicationDbContext db, IMapper mapper, IStorageService storageService)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
    }

    public async Task<GenericResponse<IEnumerable<ProductViewModel>>> GetProductsAsync(
        string searchTerm = "",
        int? categoryId = null,
        OrderBy orderBy = OrderBy.NameAsc,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int pageNumber = 1,
        int pageSize = DefaultPageSize,
        string? status = null,
        bool inluceOutOfStock = false,
        FilterBy filterBy = FilterBy.Featured
        )
    {

        if (pageNumber < 1)
        {
            pageNumber = 1;
        }
        pageSize = Math.Clamp(pageSize, 1, MaxPageSize);

        var query = _db.Products.Include(p => p.Category)
            .Include(p => p.Discount)
            .AsQueryable();

        // Apply filters
        switch (filterBy)
        {
            case FilterBy.New:
                query =  FilterByNewProducts(query);
                break;
            case FilterBy.Sale:
                query =  FilterByDiscountedProducts(query);
                break;
            case FilterBy.BestSeller:
                query =  FilterByBestSellingProducts(query);
                break;
            case FilterBy.MostReviewed:
                query =  FilterByMostReviewedProducts(query);
                break;
            case FilterBy.MostPopular:
                query =  FilterByMostPopularProducts(query);
                break;
            case FilterBy.TopRated:
                query =  FilterByTopRatedProducts(query);
                break;
            case FilterBy.Featured:
                // No additional filtering needed for featured products
                break;
        }

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

        // Apply filtering
        if (status != null)
        {
            query = query.Where(p => p.Status.ToString().ToLower() == status.ToLower());
        }
        if (inluceOutOfStock)
        {
            query = query.Where(p => p.Stock > 0);
        }
        int totalRecords = await query.CountAsync();
        int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

        var products = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return GenericResponse<IEnumerable<ProductViewModel>>.SuccessResponse(products, "Products retrieved successfully", totalRecords, pageNumber, totalPages);
    }



    public async Task<GenericResponse<ProductViewModel>> GetProductByIdAsync(int id)
    {

        var product = await _db.Products
            .Include(p => p.Category)
            .Include(p => p.Discount)
            .FirstOrDefaultAsync(p => p.Id == id);




        if (product == null)
        {
            return GenericResponse<ProductViewModel>.FailureResponse($"Product with ID {id} not found");
        }

        return GenericResponse<ProductViewModel>.SuccessResponse(
            message: "Product retrieved successfully",
            data: _mapper.Map<ProductViewModel>(product));

    }

    public async Task<GenericResponse<ProductViewModel>> CreateProductAsync(CreateProductViewModel model)
    {


        var category = await _db.Categories.FindAsync(model.CategoryId);
        if (category == null)
        {
            return GenericResponse<ProductViewModel>.FailureResponse($"Category with ID {model.CategoryId} not found");
        }

        // Validate the image file
        if (model.Image == null || model.Image.Length == 0)
        {
            return GenericResponse<ProductViewModel>.FailureResponse("Image file is required");
        }
        // Upload the image and get the URL
        using var stream = new MemoryStream();
        await model.Image.CopyToAsync(stream);
        stream.Position = 0;

        var imageRes = await _storageService.UploadPhotoAsync(
            stream,
            model.Image.FileName,
            model.Image.ContentType
        );


        if (string.IsNullOrEmpty(imageRes.fileUrl))
        {
            return GenericResponse<ProductViewModel>.FailureResponse("Image upload failed");
        }

        var product = _mapper.Map<Product>(model);

        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        return GenericResponse<ProductViewModel>.SuccessResponse(
            message: "Product created successfully",
            data: _mapper.Map<ProductViewModel>(product));


    }

    public async Task<GenericResponse<ProductViewModel>> UpdateProductAsync(UpdateProductViewModel model)
    {
        var product = await _db.Products.FindAsync(model.Id);
        if (product == null)
        {
            return GenericResponse<ProductViewModel>.FailureResponse($"Product with ID {model.Id} not found");
        }

        // Validate the image file
        if (model.Image != null && model.Image.Length > 0)
        {
            using var stream = new MemoryStream();
            await model.Image.CopyToAsync(stream);
            stream.Position = 0;

            var imageRes = await _storageService.UploadPhotoAsync(
                stream,
                model.Image.FileName,
                model.Image.ContentType
            );

            if (string.IsNullOrEmpty(imageRes.fileUrl))
            {
                return GenericResponse<ProductViewModel>.FailureResponse("Image upload failed");
            }
            product.ImageUrl = imageRes.fileUrl;
        }

        _mapper.Map(model, product);
        _db.Products.Update(product);
        await _db.SaveChangesAsync();

        return GenericResponse<ProductViewModel>.SuccessResponse(
            message: "Product updated successfully",
            data: _mapper.Map<ProductViewModel>(product));
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null)
        {
            return false;
        }

        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
        return true;
    }
    // ReturnedOrder query
    private  IQueryable<Product> FilterByNewProducts(IQueryable<Product> query)
    {
        query = query.Where(p => p.CreatedAt >= DateTime.UtcNow.AddDays(-NewProductsDays));
        return query;
    }

    private  IQueryable<Product> FilterByDiscountedProducts(IQueryable<Product> query)
    {
        query = query.Where(p => p.Discount != null && p.Discount.StartDate <= DateTime.UtcNow && p.Discount.EndDate >= DateTime.UtcNow)
            .OrderByDescending(p =>  p.Discount.Amount);
        return query;
    }

    private  IQueryable<Product> FilterByBestSellingProducts(IQueryable<Product> query)
    {
        query = query.OrderByDescending(p => p.NoOfPurchase);
        return query;
    }

    private  IQueryable<Product> FilterByMostReviewedProducts(IQueryable<Product> query)
    {
        query = query.OrderByDescending(p => p.NoOfReviews);
        return query;
    }

    private  IQueryable<Product> FilterByMostPopularProducts(IQueryable<Product> query)
    {
        query = query.OrderByDescending(p => p.NoOfViews);
        return query;
    }

    private  IQueryable<Product> FilterByTopRatedProducts(IQueryable<Product> query)
    {
        query = query.
            Where(p => p.AverageReviewScore > 2).
            OrderByDescending(p => p.AverageReviewScore);
        return query;
    }

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
