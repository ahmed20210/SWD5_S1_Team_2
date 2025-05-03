using Domain.Entities;
using Domain.DTOs.CategoryDTOs;
using AutoMapper;
using Business.ViewModels.CategoryViewModel;


namespace Business.mapper;

public class CategoryMapperProfile: Profile
{

    public CategoryMapperProfile()
    {
        CreateMap<Category, CategoryViewModel>();
       
    }
}
