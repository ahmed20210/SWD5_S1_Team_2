using AutoMapper;
using Business.ViewModels.ReviewsViewModels;
using Domain.Entities;

namespace Business.mapper;

public class ReviewMapperProfile : Profile
{
    public ReviewMapperProfile()
    {
        CreateMap<Review, ReviewViewModel>()
            .ForMember(dest => dest.UserName, opt => opt.Ignore())
            .ForMember(dest => dest.ProductName, opt => opt.Ignore());
            
        CreateMap<CreateReviewViewModel, Review>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.IsVerified, opt => opt.Ignore())
            .ForMember(dest => dest.ReviewDate, opt => opt.Ignore());
            
        CreateMap<UpdateReviewViewModel, Review>()
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.ProductId, opt => opt.Ignore())
            .ForMember(dest => dest.ReviewDate, opt => opt.Ignore());
    }
}
