
    using System.Security.Claims;
    using Domain.Entities;
    using Infrastructure.Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    namespace Web.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class FavouriteListController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public FavouriteListController(ApplicationDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToFavorites(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("User not logged in");

            var product = await _db.Products.FindAsync(productId);
            if (product == null)
                return NotFound("Product not found");

            var existingFavorite = await _db.FavouriteListS
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

            if (existingFavorite != null)
                return BadRequest("Product already in favourites");

            var favorite = new FavouriteList
            {
                UserId = userId,
                ProductId = productId,
                CreatedAt = DateTime.UtcNow
            };

            _db.FavouriteListS.Add(favorite);
            await _db.SaveChangesAsync();

            return Ok("Product added to favourites");
        }

        [HttpDelete("remove/{productId}")]
        public async Task<IActionResult> RemoveFromFavorites(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("User not logged in");

            var favorite = await _db.FavouriteListS
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

            if (favorite == null)
                return NotFound("Favorite not found");

            _db.FavouriteListS.Remove(favorite);
            await _db.SaveChangesAsync();

            return Ok("Product removed from favourites");
        }

        [HttpGet]
        public async Task<IActionResult> GetUserFavorites()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("User not logged in");

            var favorites = await _db.FavouriteListS
                .Where(f => f.UserId == userId)
                .Include(f => f.Product)
                .Select(f => new
                {
                    f.Product.Id,
                    f.Product.Name,
                    f.Product.Description,
                    f.Product.Price,
                    CategoryName = f.Product.Category.Name,
                    f.Product.CreatedAt
                })
                .ToListAsync();

            return Ok(favorites);
        }
    }


