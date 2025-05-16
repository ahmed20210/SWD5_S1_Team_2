using Domain.Entities;
using Domain.Response;
using Business.ViewModels.AddressViewModels;

namespace Business.Services.AddressService;

public interface IAddressService
{
    Task<GenericResponse<AddressViewModel>> CreateAddressAsync(CreateAddressViewModel addressViewModel, string userId);
    
    Task<GenericResponse<AddressViewModel>> UpdateAddressAsync(UpdateAddressViewModel addressViewModel, int id, string userId);
    
    Task<GenericResponse<AddressViewModel>> GetAddressByIdAsync(int id);
    
    Task<GenericResponse<List<AddressViewModel>>> GetAllUserAddressesAsync(string userId);
    
    Task<BaseResponse> DeleteAddressAsync(int id, string userId);
    
    Task<BaseResponse> SetAsMainAddressAsync(int id, string userId);
}
