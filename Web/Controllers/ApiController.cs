using System.Security.Claims;
using Business.Services.ProductService;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ApplicationDbContext _db;

        public ApiController(IProductService productService, ApplicationDbContext db)
        {
            _productService = productService;
            _db = db;
        }

        [HttpGet("products/byIds")]
        public async Task<IActionResult> GetProductsByIds([FromQuery] int[] ids)
        {
            if (ids == null || !ids.Any())
            {
                return BadRequest("No product IDs provided");
            }

            try
            {
                var currentDate = DateTime.UtcNow;
                
                // Fetch products with their discounts
                var products = await _db.Products
                    .Include(p => p.Discount)
                    .Where(p => ids.Contains(p.Id))
                    .Select(p => new 
                    {
                        p.Id,
                        p.Name,
                        p.Price,
                        p.ImageUrl,
                        Discount = p.Discount != null && 
                                p.Discount.StartDate <= currentDate && 
                                p.Discount.EndDate >= currentDate
                            ? p.Discount
                            : null
                    })
                    .ToListAsync();

                if (!products.Any())
                {
                    return NotFound("No products found with the provided IDs");
                }

                // Calculate the final prices with discounts
                var result = products.Select(p => new
                {
                    id = p.Id,
                    name = p.Name,
                    originalPrice = p.Price,
                    price = p.Discount != null 
                        ? Math.Round(p.Price - (p.Price * p.Discount.Amount / 100), 2)
                        : p.Price,
                    discountPercentage = p.Discount?.Amount ?? 0,
                    imageUrl = p.ImageUrl
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving products: " + ex.Message);
            }
        }
    }
}
