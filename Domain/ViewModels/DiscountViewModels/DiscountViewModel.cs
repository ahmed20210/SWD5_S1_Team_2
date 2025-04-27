using System.ComponentModel.DataAnnotations;
using Domain.DTOs.ProductDTOs;

namespace Domain.ViewModels.DiscountViewModels;

public class DiscountViewModel 
{
    public int Id { get; set; }
    public ProductBaseViewModel? Product { get; set; }
    public int? ProductId { get; set; }

    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool isExpired => EndDate < StartDate;

}

public class CreateDiscountViewModel 
{
    [Required(ErrorMessage = "Product ID is required")]
    [Display(Name = "Product")]
    public int ProductId { get; set; }

    [Range(0, 100, ErrorMessage = "Amount must be between 0 and 100")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Start date is required")]
    [DataType(DataType.Date)]
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "End date is required")]
    [DataType(DataType.Date)]
    [Display(Name = "End Date")]
    public DateTime EndDate { get; set; }
}

public class UpdateDiscountViewModel
{
    public int Id { get; set; }

    
    public int? ProductId { get; set; }
    
    [Range(0, 100, ErrorMessage = "Amount must be between 0 and 100")]

    public decimal? Amount { get; set; }

    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Start date is required")]
    [Display(Name = "Start Date")]
    public DateTime? StartDate { get; set; }

    [DataType(DataType.Date)]
    [Required(ErrorMessage = "End date is required")]
    [Display(Name = "End Date")]
    public DateTime? EndDate { get; set; }
}


