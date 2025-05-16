using Business.ViewModels.CategoryViewModels;
using Business.ViewModels.DiscountViewModels;
using Domain;
using Domain.DTOs.ProductDTOs;

namespace Business.ViewModels.ProductViewModels;


public class ProductViewModel  
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int? Stock { get; set; }
    public double? AverageReviewScore { get; set; }
    public ProductStatus? Status { get; set; }
    
    public CategoryViewModel? Category { get; set; }

    public DiscountViewModel? Discount { get; set; }
    public int? NoOfViews { get; set; }
    public int? NoOfPurchase { get; set; }
    
    public string? ImageUrl { get; set; }

    public DateTime? CreatedAt { get; set; } 
    public int? Rating { get; set; }
    public int? ReviewCount { get; set; }
    public int? NoOfReviews { get; set; }
    



}





public class GetAllProductsViewModel : ProductBaseViewModel
{
    public int Id { get; set; }
}

public class GetProductViewModel : ProductBaseViewModel
{

}
