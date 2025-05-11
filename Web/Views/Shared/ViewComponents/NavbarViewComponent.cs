using Business.Services.CategoryService;
using Microsoft.AspNetCore.Mvc;

namespace Web.Views.Shared.ViewComponents;

public class NavbarViewComponent : ViewComponent
{
    private readonly ICategoryService _categoryService;
    public NavbarViewComponent(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();

        return View(categories);
    }
}
