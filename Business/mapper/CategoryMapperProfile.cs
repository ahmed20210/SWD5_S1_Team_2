using Domain.Entities;
using AutoMapper;
using Business.ViewModels.CategoryViewModels;


namespace Business.mapper;

public class CategoryMapperProfile: Profile
{

    public CategoryMapperProfile()
    {
        CreateMap<Category, CategoryViewModel>();
        CreateMap<CreateCategoryViewModel, Category>();
        CreateMap<UpdateCategoryViewModel, Category>()
        .ReverseMap();
        CreateMap<CategoryViewModel, UpdateCategoryViewModel>();
       
    }
}
