using Business.ViewModels.ReviewsViewModels;
using Domain.Entities;
using Domain.Response;

namespace Business.Services.ReviewsService;

public interface IReviewService
{
    Task<GenericResponse<IEnumerable<ReviewViewModel>>> GetReviewsAsync(
        int? productId = null,
        string userId = null,
        bool? isVerified = null,
        int pageNumber = 1,
        int pageSize = 10);
    
    Task<GenericResponse<ReviewViewModel>> GetReviewByIdAsync(int id);
    
    Task<GenericResponse<ReviewViewModel>> CreateReviewAsync(CreateReviewViewModel model, string userId);
    
    Task<GenericResponse<ReviewViewModel>> UpdateReviewAsync(UpdateReviewViewModel model, string userId);
    
    Task<GenericResponse<bool>> DeleteReviewAsync(int id, string userId);
    
    Task<GenericResponse<bool>> VerifyReviewAsync(int id);
}
