using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.ViewModels.CategoryViewModel;
using Domain.DTOs.CategoryDTOs;
namespace Business.Services.CategoryService
{

    public interface ICategoryService
    {
        Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync();
        Task<CategoryViewModel> GetCategoryByIdAsync(int id);
        Task<CategoryViewModel> CreateCategoryAsync(CreateCategoryViewModel categoryViewModelForm);

        Task<CategoryViewModel> UpdateCategoryAsync(UpdateCategoryViewModel updateCategoryViewModelForm);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
