using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModels.CategoryViewModel
{
    internal class CategoryViewModel
    {
    }
    public class GetAllCategoriesViewModel
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
        public string Description { get; set; }

        [Required(ErrorMessage = "Image path is required")]
        [MaxLength(200, ErrorMessage = "Image path cannot exceed 200 characters")]
        public string ImagePath { get; set; }
    }

    public class UpdateCategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(20, ErrorMessage = "Name cannot exceed 20 characters")]
        public string Name { get; set; }

        [MaxLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Image path is required")]
        [MaxLength(200, ErrorMessage = "Image path cannot exceed 200 characters")]
        public string ImagePath { get; set; }
    }

    public class DeleteCategoryViewModel
    {
        public int Id { get; set; }
    }
}




