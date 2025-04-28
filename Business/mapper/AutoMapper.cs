

using AutoMapper;
using Domain.Entities;
using Domain.DTOs.CategoryDTOs;
namespace Business.mapper
{
    
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
        
            CreateMap<Category, GetAllCategoriesDto>();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();
        }
        
    }
}
