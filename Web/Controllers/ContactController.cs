using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Controllers
{
    public class ContactController : Controller
    {
        // You could inject an email service here to handle contact form submissions
        public ContactController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(ContactViewModel model)
        {
           
            

            return View();
        }
    }
}
