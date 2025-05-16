using Domain.DTOs.DiscountDTOs;
using Domain.Response;

namespace Business.Services.DiscountService;

public interface IDiscountService
{
  Task<GenericResponse<CreateDiscountDTO>> CreateDiscountAsync(CreateDiscountDTO discountDto);

    Task<GenericResponse<UpdateDiscountDTO>> UpdateDiscountAsync(UpdateDiscountDTO discountDto,
        int id);


    Task<GenericResponse<GetDiscountDTO>> GetDiscountByIdAsync(int id);

    Task<GenericResponse<List<GetAllDiscountsDTO>>> GetAllDiscountsAsync(
      
         int? productId,
         bool? isExpired,
         List<DateTime>? betweenDates,
           int pageNumber,
         int pageSize
         );

    Task<BaseResponse> DeleteDiscountAsync(int id);

    // Add method for bulk creating discounts for random products
    Task<BaseResponse> CreateBulkRandomDiscountsAsync(int count, decimal minAmount, decimal maxAmount, 
        DateTime startDate, DateTime endDate);
        
    // Add method for synchronizing all discounts with products
    Task<BaseResponse> SynchronizeDiscountsAsync();
}
