using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CheckoutController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Authorize] // Require authentication for checkout
        public async Task<IActionResult> Index()
        {
            

            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PlaceOrder(int addressId, string paymentMethod)
        {
          
        
            return RedirectToAction("Confirmation", new { orderId = 1 });
        }

        [Authorize]
        public async Task<IActionResult> Confirmation(int orderId)
        {
         

            return View();
        }
    }
}
