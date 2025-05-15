using Business.Services.UserService;
using Business.ViewModels.EditUserViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    // [Authorize(Roles = "Admin")]
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
                IsVerified = user.IsVerified,
                Role = user.Role
            };

            return View(viewModel);
        }

        // Handle Edit form submission
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            
            var user = await _userService.GetUserByIdAsync(viewModel.Id);
            if (user == null) return NotFound();

            user.FName = viewModel.FName;
            user.LName = viewModel.LName;
            user.Email = viewModel.Email;
            user.IsVerified = viewModel.IsVerified;
            // Don't allow changing the role through the standard edit form

            var result = await _userService.UpdateUserAsync(user);

            if (result)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "An error occurred while updating the user.");
            return View(viewModel);
        }

        // Show suspension confirmation page
        public async Task<IActionResult> Suspend(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            
            return View(user);
        }
        
        // Process user suspension after confirmation
        [HttpPost]
        public async Task<IActionResult> Suspend(string id, bool confirm)
        {
            if (confirm)
            {
                var result = await _userService.SuspendUserAsync(id);
                if (!result)
                {
                    TempData["ErrorMessage"] = "Failed to suspend user.";
                }
                else
                {
                    TempData["SuccessMessage"] = "User has been suspended successfully.";
                }
            }
            
            return RedirectToAction("Index");
        }
        
        // Show activation confirmation page
        public async Task<IActionResult> Activate(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            
            return View(user);
        }
        
        // Process user activation after confirmation
        [HttpPost]
        public async Task<IActionResult> Activate(string id, bool confirm)
        {
            if (confirm)
            {
                var result = await _userService.ActivateUserAsync(id);
                if (!result)
                {
                    TempData["ErrorMessage"] = "Failed to activate user.";
                }
                else
                {
                    TempData["SuccessMessage"] = "User has been activated successfully.";
                }
            }
            
            return RedirectToAction("Index");
        }

        // Display the change role form
        public async Task<IActionResult> ChangeRole(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            
            var viewModel = new ChangeRoleViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                CurrentRole = user.Role,
                NewRole = user.Role
            };
            
            return View(viewModel);
        }
        
        // Process role change
        [HttpPost]
        public async Task<IActionResult> ChangeRole(ChangeRoleViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            
            // Only proceed if the role has actually changed
            if (viewModel.CurrentRole != viewModel.NewRole)
            {
                var result = await _userService.ChangeUserRoleAsync(viewModel.Id, viewModel.NewRole);
                if (result)
                {
                    TempData["SuccessMessage"] = $"User role has been changed to {viewModel.NewRole}.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to change user role.";
                    return View(viewModel);
                }
            }
            
            return RedirectToAction("Index");
        }
    }
}
