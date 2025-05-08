using Business.Services.UserService;
using Business.ViewModels.EditUserViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Web.Areas.Admin.Controllers
{


    [Area("Admin")]
    
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Display all users
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        // Load user data into the Edit form
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            var viewModel = new EditUserViewModel
            {
                Id = user.Id,
                FName = user.FName,
                LName = user.LName,
                Email = user.Email,
                IsVerified = user.IsVerified
            };

            return View(viewModel);
        }

        // Handle Edit form submission
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel viewModel)
        {
            var user = await _userService.GetUserByIdAsync(viewModel.Id);
            if (user == null) return NotFound();

            user.FName = viewModel.FName;
            user.LName = viewModel.LName;
            user.Email = viewModel.Email;
            user.IsVerified = viewModel.IsVerified;

            var result = await _userService.UpdateUserAsync(user);

            if (result)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "An error occurred while updating the user.");
            return View(viewModel);
        }

        // Suspend a user account
        public async Task<IActionResult> Suspend(string id)
        {
            var result = await _userService.SuspendUserAsync(id);
            return RedirectToAction("Index");
        }

    }
}
