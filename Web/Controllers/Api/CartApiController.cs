// Create a CartApiController.cs file
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using System.Text.Json;

namespace Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CartApiController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("GetCount")]
        public IActionResult GetCount()
        {
            // For non-logged in users, we'll return 0 and let the client-side JavaScript
            // handle getting the count from localStorage
            if (!User.Identity.IsAuthenticated)
            {
                return Ok(new { count = 0 });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            // For logged-in users, we should query the database
            // Since you're currently using localStorage for the cart, we'll return 0
            // In a real implementation, you would query your database here
            return Ok(new { count = 0 });
        }
    }
}
