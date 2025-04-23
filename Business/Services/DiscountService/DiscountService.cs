using AutoMapper;
using Domain.DTOs.DiscountDTOs;
using Domain.DTOs.ProductDTOs;
using Domain.Entities;
using Domain.Response;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Business.Services.DiscountService;

public class DiscountService: IDiscountService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;


    public DiscountService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper;
    }
    public async Task<GenericResponse<CreateDiscountDTO>> CreateDiscountAsync(CreateDiscountDTO discountDto)
    {
       try
       {
        

            if (discountDto.Amount <= 0)
            {
                return new GenericResponse<CreateDiscountDTO>
                {
                    Success = false,
                    Message = "Discount percentage must be greater than zero.",
                    Data = null
                };
            }
            if (discountDto.StartDate >= discountDto.EndDate)
            {
                return new GenericResponse<CreateDiscountDTO>
                {
                    Success = false,
                    Message = "Start date must be earlier than end date.",
                    Data = null
                };
            }
            if (discountDto.ProductId <= 0)
            {
                return new GenericResponse<CreateDiscountDTO>
                {
                    Success = false,
                    Message = "Product ID is required.",
                    Data = null
                };
            }

            Discount discount = new Discount
            {
                Amount = discountDto.Amount,
                StartDate = discountDto.StartDate,
                EndDate = discountDto.EndDate,
                ProductId = discountDto.ProductId
            };

            await _context.Discounts.AddAsync(discount);
            await _context.SaveChangesAsync();

            var product = await _context.Products.FindAsync(discount.ProductId);

            return new GenericResponse<CreateDiscountDTO>
            {
                Success = true,
                Message = "Discount created successfully.",
                Data = new CreateDiscountDTO
                {
                    Amount = discount.Amount,
                    StartDate = discount.StartDate,
                    EndDate = discount.EndDate,
                    ProductId = discount.ProductId,
                    Product = product != null ? new ProductBaseDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        Description = product.Description,
                        
                    } : null


                }
            };
        }
        catch (Exception ex)
        {
            return new GenericResponse<CreateDiscountDTO>
            {
                Success = false,
                Message = $"An error occurred while creating the discount: {ex.Message}",
                Data = null
            };
        }

    
    }

    public async Task<GenericResponse<UpdateDiscountDTO>> UpdateDiscountAsync(UpdateDiscountDTO discountDto, int id)
    {
        try
        {
            // get the discount by id
            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null)
            {
                return new GenericResponse<UpdateDiscountDTO>
                {
                    Success = false,
                    Message = "Discount not found.",
                    Data = null
                };
            }

            // update the discount properties
            discount.Amount = discountDto.Amount ?? discount.Amount;
            discount.StartDate = discountDto.StartDate ?? discount.StartDate;
            discount.EndDate = discountDto.EndDate ?? discount.EndDate;
            discount.ProductId = discountDto.ProductId ?? discount.ProductId;

            _context.Discounts.Update(discount);
            await _context.SaveChangesAsync();

            return new GenericResponse<UpdateDiscountDTO>
            {
                Success = true,
                Message = "Discount updated successfully.",
                Data = new UpdateDiscountDTO
                {
                    Id = discount.Id,
                    Amount = discount.Amount,
                    StartDate = discount.StartDate,
                    EndDate = discount.EndDate,
                    ProductId = discount.ProductId
                }
            };
        }
        catch (Exception ex)
        {
            return new GenericResponse<UpdateDiscountDTO>
            {
                Success = false,
                Message = $"An error occurred while updating the discount: {ex.Message}",
                Data = null
            };
        }
    }



    public Task<GenericResponse<GetDiscountDTO>> GetDiscountByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<GenericResponse<List<GetAllDiscountsDTO>>> GetAllDiscountsAsync(
     int? productId, bool? isExpired, List<DateTime>? betweenDates, int pageNumber,
     int pageSize)
    {
        try
        {
            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }
            var query = _context.Discounts.AsQueryable();
            if (productId.HasValue)
            {
                query = query.Where(d => d.ProductId == productId.Value);
            }

            if (isExpired.HasValue)
            {
                var currentDate = DateTime.UtcNow;
                query = isExpired.Value ? query.Where(d => d.EndDate < currentDate) : query.Where(d => d.EndDate >= currentDate);
            }

            if (betweenDates != null && betweenDates.Count == 2)
            {
                query = query.Where(d => d.StartDate >= betweenDates[0] && d.EndDate <= betweenDates[1]);
            }
            Console.WriteLine($"PageNumber: {pageNumber}, PageSize: {pageSize}, ProductId: {productId}, IsExpired: {isExpired}, BetweenDates: {betweenDates}");

            var totalRecords = await query.CountAsync();

            var discounts = await query
                .Include(d => d.Product) // Include the related Product entity
                .OrderByDescending(d => d.StartDate)
                .ThenBy(d => d.EndDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new GetAllDiscountsDTO
                {
                    Id = d.Id,
                    Amount = d.Amount,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                    ProductId = d.ProductId,
                    Product = new ProductBaseDto
                    {
                        Id = d.Product.Id,
                        Name = d.Product.Name,
                        Price = d.Product.Price,
                        Description = d.Product.Description
                    }
                })
                .ToListAsync();
              

            

            return new GenericResponse<List<GetAllDiscountsDTO>>
            {
                Success = true,
                Message = "Discounts retrieved successfully.",
                Data = discounts,
                TotalCount = totalRecords
            };
        }
        catch (Exception ex)
        {
            return new GenericResponse<List<GetAllDiscountsDTO>>
            {
                Success = false,
                Message = $"An error occurred while retrieving discounts: {ex.Message}",
                Data = null
            };
        }
    }

    public Task<BaseResponse> DeleteDiscountAsync(int id)
    {
        try
        {
            var discount = _context.Discounts.Find(id);
            if (discount == null)
            {
                return Task.FromResult(new BaseResponse
                {
                    Success = false,
                    Message = "Discount not found."
                });
            }

            _context.Discounts.Remove(discount);
            _context.SaveChanges();

            return Task.FromResult(new BaseResponse
            {
                Success = true,
                Message = "Discount deleted successfully."
            });
        }
        catch (Exception ex)
        {
            return Task.FromResult(new BaseResponse
            {
                Success = false,
                Message = $"An error occurred while deleting the discount: {ex.Message}"
            });
        }
    }

}
