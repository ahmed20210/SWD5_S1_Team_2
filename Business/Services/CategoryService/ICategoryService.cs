using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using Domain.DTOs.CategoryDTOs;
namespace Business.Services.CategoryService
{

    public interface ICategoryService
    {
        Task<IEnumerable<GetAllCategoriesDto>> GetAllCategoriesAsync();
        Task<GetAllCategoriesDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto);
        Task<GetAllCategoriesDto> UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto);
        Task<bool> DeleteCategoryAsync(DeleteCategoryDto deleteCategoryDto);
    }
}
