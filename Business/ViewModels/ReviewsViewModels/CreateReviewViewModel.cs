using System.ComponentModel.DataAnnotations;

namespace Business.ViewModels.ReviewsViewModels;

public class CreateReviewViewModel
{
    [Required]
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
    public int Rating { get; set; }
    
    [MaxLength(500, ErrorMessage = "Comment must be less than 500 characters")]
    public string Comment { get; set; }
    
    [Required]
    public int ProductId { get; set; }
}
