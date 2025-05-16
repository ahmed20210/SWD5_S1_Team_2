using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class StoreController : Controller
    {
        public IActionResult Locations()
        {
            // Define our three store locations with coordinates
            var storeLocations = new List<StoreLocationViewModel>
            {
                new StoreLocationViewModel
                {
                    Name = "TechXpress Downtown",
                    Address = "123 Main Street, New York, NY 10001",
                    PhoneNumber = "(212) 555-1234",
                    OpeningHours = "Mon-Sat: 9AM-9PM, Sun: 10AM-6PM",
                    Latitude = 40.7128,
                    Longitude = -74.0060,
                    Description = "Our flagship store with the largest selection of products."
                },
                new StoreLocationViewModel
                {
                    Name = "TechXpress Uptown",
                    Address = "456 Park Avenue, New York, NY 10022",
                    PhoneNumber = "(212) 555-5678",
                    OpeningHours = "Mon-Sat: 10AM-8PM, Sun: 11AM-6PM",
                    Latitude = 40.7644,
                    Longitude = -73.9695,
                    Description = "Boutique store specializing in premium tech products."
                },
                new StoreLocationViewModel
                {
                    Name = "TechXpress Brooklyn",
                    Address = "789 Atlantic Avenue, Brooklyn, NY 11217",
                    PhoneNumber = "(718) 555-9012",
                    OpeningHours = "Mon-Sat: 10AM-8PM, Sun: 11AM-7PM",
                    Latitude = 40.6782,
                    Longitude = -73.9442,
                    Description = "Our newest store featuring our complete product line and repair services."
                }
            };
            
            return View(storeLocations);
        }
    }
}