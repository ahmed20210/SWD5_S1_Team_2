using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Web.Areas.Admin.Controllers;

// [Authorize(Roles = "Admin")]
[Area("Admin")]
public class DashboardController : Controller
{
    public ActionResult Index()
    {
        return View();
    }
}
