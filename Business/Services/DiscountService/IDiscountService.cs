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
        int pageNumber,
         int pageSize,
         int? productId,
         bool? isExpired,
         List<DateTime>? betweenDates
         );

    Task<BaseResponse> DeleteDiscountAsync(int id);

    
}
