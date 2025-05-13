using Domain.Entities;

namespace Business.Services.FavouriteListService
{
    public interface IFavouriteListService
    {
        Task<bool> AddToFavouritesAsync(string userId, int productId);
        Task<bool> RemoveFromFavouritesAsync(string userId, int productId);
        Task<List<Product>> GetUserFavouritesAsync(string userId);
    }
}
