using AutoMapper;
using Business.ViewModels.ProductViewModels;
using Domain.Entities;

namespace Business.mapper;

public class ProductMapperProfile : Profile
{
    public ProductMapperProfile()
    {
        CreateMap<Product, ProductViewModel>();


    }
}
