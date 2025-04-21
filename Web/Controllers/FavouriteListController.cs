using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Web.Models;
using System.Security.Claims;

namespace Web.Controllers
{
    public class FavouriteListController : Controller
    {
        private readonly ApplicationDbContext _db;

        public FavouriteListController(ApplicationDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToFavorites(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var product = await _db.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var existingFavorite = await _db.FavouriteListS
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);
            if (existingFavorite != null)
            {
                return RedirectToAction(nameof(Index));
            }

            var favorite = new FavouriteList
            {
                UserId = userId,
                ProductId = productId,
                CreatedAt = DateTime.UtcNow
            };

            _db.FavouriteListS.Add(favorite);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromFavorites(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var favorite = await _db.FavouriteListS
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);
            if (favorite == null)
            {
                return NotFound();
            }

            _db.FavouriteListS.Remove(favorite);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var favorites = await _db.FavouriteListS
                .Where(f => f.UserId == userId)
                .Include(f => f.Product)
                .Select(f => f.Product)
                .ToListAsync();

            return View(favorites);
        }
    }
}

