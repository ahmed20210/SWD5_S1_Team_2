using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Business.ViewModels.CategoryViewModels;

public class UpdateCategoryViewModel : CreateCategoryViewModel
{
    public int Id { get; set; }

    [Display(Name = "Category Image")]
    [DataType(DataType.Upload)]
    // [FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Please upload a valid image file (jpg, jpeg, png)")]
    public new IFormFile? Image { get; set; } // make image optional


}
