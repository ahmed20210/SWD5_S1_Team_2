using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using System.Security.Claims;
using Business.Services.FavouriteListService;

namespace Web.Controllers
{
    public class FavouriteListController : Controller
    {
        private readonly IFavouriteListService _favouriteListService;

        public FavouriteListController(IFavouriteListService favouriteListService)
        {
            _favouriteListService = favouriteListService ?? throw new ArgumentNullException(nameof(favouriteListService));
        }

        [HttpGet]
        public async Task<IActionResult> GetCount()
        {
            // If user is not logged in, return 0
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { count = 0 });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favorites = await _favouriteListService.GetUserFavouritesAsync(userId);
            
            return Json(new { count = favorites?.Count() ?? 0 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToFavorites(int productId, string returnUrl)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _favouriteListService.AddToFavouritesAsync(userId, productId);
            if (!result)
            {
                // If this is an AJAX request, return JSON
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Failed to add to favorites" });
                }
                return NotFound();
            }

            // If this is an AJAX request, return JSON
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Added to favorites" });
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromFavorites(int productId, string returnUrl)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _favouriteListService.RemoveFromFavouritesAsync(userId, productId);
            if (!result)
            {
                // If this is an AJAX request, return JSON
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Failed to remove from favorites" });
                }
                return NotFound();
            }

            // If this is an AJAX request, return JSON
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Removed from favorites" });
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var favorites = await _favouriteListService.GetUserFavouritesAsync(userId);

            return View(favorites);
        }
    }
}

