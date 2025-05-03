using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data; 
using AutoMapper; 
using System.Threading.Tasks;
using Business.Services.CategoryService;
using Domain.DTOs.CategoryDTOs;
using Business.ViewModels.CategoryViewModel;

namespace Web.Controllers 
{
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

       
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return View(categories);
        }

        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryViewModel createCategoryForm)
        {

            if (ModelState.IsValid)
            {
                await _categoryService.CreateCategoryAsync(createCategoryForm);
                return RedirectToAction(nameof(Index));
            }
            return View(createCategoryForm);
        }

       
        public async Task<IActionResult> Update(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();
                
            var updateCategoryViewModel = _mapper.Map<UpdateCategoryViewModel>(category);
            return View(updateCategoryViewModel);
        }

       
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateCategoryViewModel updateCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryService.UpdateCategoryAsync(updateCategoryViewModel);
                if (result == null)
                    return NotFound();
                return RedirectToAction(nameof(Index));
            }
            return View(updateCategoryViewModel);
        }

       
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();

            return View(id);
        }

      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result)
            {
                TempData["Error"] = "Cannot delete category because it has associated products.";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
