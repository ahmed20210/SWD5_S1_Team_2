using AutoMapper;
using Business.ViewModels.DiscountViewModels;
using Domain.Entities;

namespace Business.mapper;

public class DiscoutMapperProfile : Profile
{

    public DiscoutMapperProfile()
    {
        CreateMap<Discount, DiscountViewModel>();
        CreateMap<CreateDiscountViewModel, Discount>();
        CreateMap<UpdateDiscountViewModel, Discount>();
    }
}
