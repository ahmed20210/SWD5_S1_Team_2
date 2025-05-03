using AutoMapper;
using Domain.Entities;
using Business.ViewModels.UserViewModel;

namespace Business.mapper;

public class AccountMapperProfile : Profile
{
    public AccountMapperProfile()
    {
        CreateMap<SignUpViewModel, User>();
    }
}
