using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Business.Services.FavouriteListService
{
    public class FavouriteListService : IFavouriteListService
    {
        private readonly ApplicationDbContext _dbContext;

        public FavouriteListService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<bool> AddToFavouritesAsync(string userId, int productId)
        {
            // Check if product exists
            var product = await _dbContext.Products.FindAsync(productId);
            if (product == null)
            {
                return false;
            }

            // Check if already in favorites
            var existingFavorite = await _dbContext.FavouriteListS
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);
            if (existingFavorite != null)
            {
                return true; // Already in favorites
            }

            // Add to favorites
            var favorite = new FavouriteList
            {
                UserId = userId,
                ProductId = productId,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.FavouriteListS.Add(favorite);
            await _dbContext.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> RemoveFromFavouritesAsync(string userId, int productId)
        {
            var favorite = await _dbContext.FavouriteListS
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);
                
            if (favorite == null)
            {
                return false;
            }

            _dbContext.FavouriteListS.Remove(favorite);
            await _dbContext.SaveChangesAsync();
            
            return true;
        }

        public async Task<List<Product>> GetUserFavouritesAsync(string userId)
        {
            return await _dbContext.FavouriteListS
                .Where(f => f.UserId == userId)
                .Include(f => f.Product)
                .Select(f => f.Product)
                .ToListAsync();
        }
    }
}
