using System.Security.Claims;
using Business.Services.ReviewsService;
using Business.ViewModels.ReviewsViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class ReviewsController : Controller
{
    private readonly IReviewService _reviewService;

    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    // Show reviews for a product
    public async Task<IActionResult> ProductReviews(int productId, int pageNumber = 1)
    {
        var result = await _reviewService.GetReviewsAsync(
            productId: productId, 
            isVerified: true,
            pageNumber: pageNumber);

        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction("Index", "Product");
        }

        ViewBag.ProductId = productId;
        ViewBag.CurrentPage = pageNumber;
        ViewBag.TotalPages = result.TotalPages;
        
        return View(result.Data);
    }
    
    // Show review form
    [Authorize]
    public IActionResult Create(int productId)
    {
        var model = new CreateReviewViewModel { ProductId = productId };
        return View(model);
    }

    // Submit new review
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateReviewViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _reviewService.CreateReviewAsync(model, userId);

        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return View(model);
        }

        TempData["Success"] = "Review submitted successfully! It will be visible after verification.";
        return RedirectToAction("Detail", "Product", new { id = model.ProductId });
    }
    
    // Show user reviews
    [Authorize]
    public async Task<IActionResult> MyReviews(int pageNumber = 1)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _reviewService.GetReviewsAsync(userId: userId, pageNumber: pageNumber);

        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction("Index", "Home");
        }

        ViewBag.CurrentPage = pageNumber;
        ViewBag.TotalPages = result.TotalPages;
        
        return View(result.Data);
    }
    
    // Show edit form
    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _reviewService.GetReviewByIdAsync(id);

        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction("MyReviews");
        }

        // Check if the user owns the review
        if (result.Data.UserId != userId)
        {
            TempData["Error"] = "You don't have permission to edit this review";
            return RedirectToAction("MyReviews");
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

    // Submit edited review
    [HttpPost]
    [Authorize]
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
        return RedirectToAction("MyReviews");
    }

    // Delete review
    [HttpPost]
    [Authorize]
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

        return RedirectToAction("MyReviews");
    }
}
