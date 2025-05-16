using System.ComponentModel.DataAnnotations;

namespace Business.ViewModels.ReviewsViewModels;

public class ReviewViewModel
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public bool IsVerified { get; set; }
    public DateTime ReviewDate { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
}
