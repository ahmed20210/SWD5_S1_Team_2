using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Business.ViewModels.CategoryViewModel
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
    }

    public class CreateCategoryViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(20, ErrorMessage = "Name cannot exceed 20 characters")]
        public string Name { get; set; }

        [MaxLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Category image is required")]
        [Display(Name = "Category Image")]
        [DataType(DataType.Upload)]
        // [FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Please upload a valid image file (jpg, jpeg, png)")]
        public IFormFile? Image { get; set; }

        public string? ImagePath { get; set; }
    }

    public class UpdateCategoryViewModel: CreateCategoryViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Category Image")]
        [DataType(DataType.Upload)]
        // [FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Please upload a valid image file (jpg, jpeg, png)")]
        public new IFormFile? Image { get; set; } // make image optional


    }



    public class DeleteCategoryViewModel
    {
        public int Id { get; set; }
    }
}




