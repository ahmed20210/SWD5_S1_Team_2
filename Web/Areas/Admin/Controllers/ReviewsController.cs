using System.Security.Claims;
using Business.Services.ReviewsService;
using Business.ViewModels.ReviewsViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers;

[Area("Admin")]
public class ReviewsController : Controller
{
    private readonly IReviewService _reviewService;

    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }
    
    // List all reviews with filters
    public async Task<IActionResult> Index(int? productId = null, bool? isVerified = null, int pageNumber = 1)
    {
        var result = await _reviewService.GetReviewsAsync(
            productId: productId, 
            isVerified: isVerified,
            pageNumber: pageNumber);

        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return View(new List<ReviewViewModel>());
        }

        ViewBag.ProductId = productId;
        ViewBag.IsVerified = isVerified;
        ViewBag.CurrentPage = pageNumber;
        ViewBag.TotalPages = result.TotalPages;
        
        return View(result.Data);
    }
    
    // Show review details
    public async Task<IActionResult> Details(int id)
    {
        var result = await _reviewService.GetReviewByIdAsync(id);

        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction("Index");
        }

        return View(result.Data);
    }
    
    // Show edit form for admin
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _reviewService.GetReviewByIdAsync(id);

        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction("Index");
        }

        var model = new UpdateReviewViewModel
        {
            Id = result.Data.Id,
            Rating = result.Data.Rating,
            Comment = result.Data.Comment,
            IsVerified = result.Data.IsVerified
        };

        return View(model);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateReviewViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _reviewService.UpdateReviewAsync(model, userId);

        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return View(model);
        }

        TempData["Success"] = "Review updated successfully!";
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Verify(int id)
    {
        var result = await _reviewService.VerifyReviewAsync(id);

        if (!result.Success)
        {
            TempData["Error"] = result.Message;
        }
        else
        {
            TempData["Success"] = "Review verified successfully!";
        }

        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _reviewService.DeleteReviewAsync(id, userId);

        if (!result.Success)
        {
            TempData["Error"] = result.Message;
        }
        else
        {
            TempData["Success"] = "Review deleted successfully!";
        }

        return RedirectToAction("Index");
    }
}
