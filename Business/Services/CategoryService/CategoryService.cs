using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using Domain.DTOs.CategoryDTOs;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace Business.Services.CategoryService
{

    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoryService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllCategoriesDto>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return _mapper.Map<IEnumerable<GetAllCategoriesDto>>(categories);
        }

        public async Task<GetAllCategoriesDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            var category = _mapper.Map<Category>(createCategoryDto);
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return _mapper.Map<GetAllCategoriesDto>(category);
        }

        public async Task<GetAllCategoriesDto> UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto)
        {
            var category = await _context.Categories.FindAsync(updateCategoryDto.Id);
            if (category == null)
                return null;

            _mapper.Map(updateCategoryDto, category);
            await _context.SaveChangesAsync();
            return _mapper.Map<GetAllCategoriesDto>(category);
        }

        public async Task<bool> DeleteCategoryAsync(DeleteCategoryDto deleteCategoryDto)
        {
            var category = await _context.Categories.FindAsync(deleteCategoryDto.Id);
            if (category == null)
                return false;

            try
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
               
                return false;
            }
        }
    }
}
