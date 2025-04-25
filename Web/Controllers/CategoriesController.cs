using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data; 
using AutoMapper; 
using System.Threading.Tasks;
using Business.Services.CategoryService;
using Domain.DTOs.CategoryDTOs;

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
        public async Task<IActionResult> Create(CreateCategoryDto createCategoryDto)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.CreateCategoryAsync(createCategoryDto);
                return RedirectToAction(nameof(Index));
            }
            return View(createCategoryDto);
        }

       
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            var updateCategoryDto = _mapper.Map<UpdateCategoryDto>(category);
            return View(updateCategoryDto);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateCategoryDto updateCategoryDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryService.UpdateCategoryAsync(updateCategoryDto);
                if (result == null)
                    return NotFound();
                return RedirectToAction(nameof(Index));
            }
            return View(updateCategoryDto);
        }

       
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            var deleteCategoryDto = new DeleteCategoryDto { Id = id };
            ViewBag.CategoryName = category.Name;
            return View(deleteCategoryDto);
        }

      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DeleteCategoryDto deleteCategoryDto)
        {
            var result = await _categoryService.DeleteCategoryAsync(deleteCategoryDto);
            if (!result)
            {
                TempData["Error"] = "Cannot delete category because it has associated products.";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
