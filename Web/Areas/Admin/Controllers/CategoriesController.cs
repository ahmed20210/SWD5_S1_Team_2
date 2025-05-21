using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Data; 
using AutoMapper; 
using Business.Services.CategoryService;
using Business.ViewModels.CategoryViewModels;

namespace Web.Areas.Admin.Controllers 
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ApplicationDbContext _context; 
        private readonly IMapper _mapper; 

        public CategoriesController(ICategoryService categoryService, ApplicationDbContext context, IMapper mapper)
        {
            _categoryService = categoryService;
            _context = context; 
            _mapper = mapper; 
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                return View(categories);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred while retrieving categories: {ex.Message}";
                return View(new List<CategoryViewModel>());
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryViewModel createCategoryForm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _categoryService.CreateCategoryAsync(createCategoryForm);
                    TempData["Success"] = "Category created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "There was an error creating the category. Please check your inputs.";
                return View(createCategoryForm);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred while creating the category: {ex.Message}";
                return View(createCategoryForm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    TempData["Error"] = "Category not found.";
                    return NotFound();
                }
                // Map the category to the UpdateCategoryViewModel
                var updateCategoryViewModel = _mapper.Map<UpdateCategoryViewModel>(category);  
                return View(updateCategoryViewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred while retrieving the category: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateCategoryViewModel updateCategoryViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _categoryService.UpdateCategoryAsync(updateCategoryViewModel);
                    if (result == null)
                    {
                        TempData["Error"] = "Category not found or could not be updated.";
                        return NotFound();
                    }
                    TempData["Success"] = "Category updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "There was an error updating the category. Please check your inputs.";
                return View(updateCategoryViewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred while updating the category: {ex.Message}";
                return View(updateCategoryViewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    TempData["Error"] = "Category not found.";
                    return NotFound();
                }

                return View(category);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _categoryService.DeleteCategoryAsync(id);
                if (!result)
                {
                    TempData["Error"] = "Cannot delete category because it has associated products.";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Success"] = "Category deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred while deleting the category: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
