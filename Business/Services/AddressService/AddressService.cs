using AutoMapper;
using Business.ViewModels.AddressViewModels;
using Domain.Entities;
using Domain.Response;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Services.AddressService;

public class AddressService : IAddressService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public AddressService(ApplicationDbContext context, IMapper mapper, UserManager<User> userManager)
    {
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<GenericResponse<AddressViewModel>> CreateAddressAsync(CreateAddressViewModel addressViewModel, string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return GenericResponse<AddressViewModel>.FailureResponse("User not found");
            }

            var address = _mapper.Map<Address>(addressViewModel);
            address.UserId = userId;

            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();

            var responseViewModel = _mapper.Map<AddressViewModel>(address);

            // Check if this is the user's first address, if so make it the main address
            if (!await _context.Addresses.AnyAsync(a => a.UserId == userId && a.Id != address.Id))
            {
                user.MainAddressId = address.Id;
                await _userManager.UpdateAsync(user);
                responseViewModel.IsMainAddress = true;
            }
            else
            {
                responseViewModel.IsMainAddress = user.MainAddressId == address.Id;
            }

            return GenericResponse<AddressViewModel>.SuccessResponse(message: "Address created successfully", data: responseViewModel);
        }
        catch (Exception ex)
        {
            return GenericResponse<AddressViewModel>.FailureResponse($"Failed to create address: {ex.Message}");
        }
    }

    public async Task<GenericResponse<AddressViewModel>> UpdateAddressAsync(UpdateAddressViewModel addressViewModel, int id, string userId)
    {
        try
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
            if (address == null)
            {
                return GenericResponse<AddressViewModel>.FailureResponse("Address not found or you don't have permission to update it");
            }

            _mapper.Map(addressViewModel, address);
            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(userId);
            var responseViewModel = _mapper.Map<AddressViewModel>(address);
            responseViewModel.IsMainAddress = user.MainAddressId == address.Id;

            return GenericResponse<AddressViewModel>.SuccessResponse(message: "Address updated successfully", data: responseViewModel);
        }
        catch (Exception ex)
        {
            return GenericResponse<AddressViewModel>.FailureResponse($"Failed to update address: {ex.Message}");
        }
    }

    public async Task<GenericResponse<AddressViewModel>> GetAddressByIdAsync(int id)
    {
        try
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
            {
                return GenericResponse<AddressViewModel>.FailureResponse("Address not found");
            }

            var user = await _userManager.FindByIdAsync(address.UserId);
            var addressViewModel = _mapper.Map<AddressViewModel>(address);
            addressViewModel.IsMainAddress = user?.MainAddressId == address.Id;

            return GenericResponse<AddressViewModel>.SuccessResponse(message: "Address retrieved successfully", data: addressViewModel);
        }
        catch (Exception ex)
        {
            return GenericResponse<AddressViewModel>.FailureResponse($"Failed to retrieve address: {ex.Message}");
        }
    }

    public async Task<GenericResponse<List<AddressViewModel>>> GetAllUserAddressesAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return GenericResponse<List<AddressViewModel>>.FailureResponse("User not found");
            }

            var addresses = await _context.Addresses
                .Where(a => a.UserId == userId)
                .ToListAsync();

            var addressViewModels = _mapper.Map<List<AddressViewModel>>(addresses);

            // Mark main address
            if (user.MainAddressId.HasValue)
            {
                foreach (var viewModel in addressViewModels)
                {
                    viewModel.IsMainAddress = viewModel.Id == user.MainAddressId.Value;
                }
            }

            return GenericResponse<List<AddressViewModel>>.SuccessResponse(message: "Addresses retrieved successfully", data: addressViewModels);
        }
        catch (Exception ex)
        {
            return GenericResponse<List<AddressViewModel>>.FailureResponse($"Failed to retrieve addresses: {ex.Message}");
        }
    }

    public async Task<BaseResponse> DeleteAddressAsync(int id, string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BaseResponse.FailureResponse("User not found");
            }

            var address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
            if (address == null)
            {
                return BaseResponse.FailureResponse("Address not found or you don't have permission to delete it");
            }

            // If trying to delete main address
            if (user.MainAddressId == id)
            {
                // Find another address to set as main, or set to null if no other addresses
                var alternativeAddress = await _context.Addresses
                    .Where(a => a.UserId == userId && a.Id != id)
                    .FirstOrDefaultAsync();

                user.MainAddressId = alternativeAddress?.Id;
                await _userManager.UpdateAsync(user);
            }

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();

            return BaseResponse.SuccessResponse("Address deleted successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse.FailureResponse($"Failed to delete address: {ex.Message}");
        }
    }

    public async Task<BaseResponse> SetAsMainAddressAsync(int id, string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BaseResponse.FailureResponse("User not found");
            }

            var address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
            if (address == null)
            {
                return BaseResponse.FailureResponse("Address not found or you don't have permission to update it");
            }

            user.MainAddressId = id;
            await _userManager.UpdateAsync(user);

            return BaseResponse.SuccessResponse("Main address set successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse.FailureResponse($"Failed to set main address: {ex.Message}");
        }
    }
}
