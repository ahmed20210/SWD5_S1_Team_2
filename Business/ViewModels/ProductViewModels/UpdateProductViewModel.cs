using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Business.ViewModels.ProductViewModels;

public class UpdateProductViewModel : CreateProductViewModel
{
    public int Id { get; set; }

    [Display(Name = "Product Image")]
    [DataType(DataType.Upload)]
    public new IFormFile? Image { get; set; } = null!;
    public string? ImageUrl { get; set; } = null!;

}
