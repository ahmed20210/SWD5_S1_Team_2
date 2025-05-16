using AutoMapper;
using Domain.Entities;
using Business.ViewModels.AddressViewModels;

namespace Business.mapper;

public class AddressMapperProfile : Profile
{
    public AddressMapperProfile()
    {
        CreateMap<Address, AddressViewModel>()
            .ForMember(dest => dest.IsMainAddress, opt => opt.Ignore());
        
        CreateMap<CreateAddressViewModel, Address>();
        CreateMap<UpdateAddressViewModel, Address>();
    }
}
