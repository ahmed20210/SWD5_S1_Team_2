using System.ComponentModel.DataAnnotations;
using Domain;
using Microsoft.AspNetCore.Http;

namespace Business.ViewModels.ProductViewModels;

public class CreateProductViewModel
{

    [Required]
    public string? Name { get; set; }

    [Required]
    [StringLength(1000, MinimumLength = 10)]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }
    
    [Required]
    public decimal Price { get; set; }
    public int Stock { get; set; }
    // public double? AverageReviewScore { get; set; }
    public ProductStatus? Status { get; set; }

    [Required]
    [Display(Name = "Product Image")]
    [DataType(DataType.Upload)]
    public IFormFile Image { get; set; }

    [Required]
    public string? ImageUrl { get; set; }

    [Required]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }


}
