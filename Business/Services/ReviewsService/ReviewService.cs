using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Services.ProductService;
using Business.ViewModels.ReviewsViewModels;
using Domain;
using Domain.Entities;
using Domain.Response;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Business.Services.ReviewsService;

public class ReviewService : IReviewService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IProductService _productService;
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 50;

    public ReviewService(
        ApplicationDbContext dbContext,
        IMapper mapper,
        IProductService productService)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
    }

    public async Task<GenericResponse<IEnumerable<ReviewViewModel>>> GetReviewsAsync(
        int? productId = null,
        string userId = null,
        bool? isVerified = null,
        int pageNumber = 1,
        int pageSize = 10)
    {
        if (pageNumber < 1) pageNumber = 1;
        pageSize = Math.Clamp(pageSize, 1, MaxPageSize);

        var query = _dbContext.Reviews
            .Include(r => r.User)
            .AsQueryable();

        // Apply filters
        if (productId.HasValue)
        {
            query = query.Where(r => r.ProductId == productId.Value);
        }

        if (!string.IsNullOrEmpty(userId))
        {
            query = query.Where(r => r.UserId == userId);
        }

        if (isVerified.HasValue)
        {
            query = query.Where(r => r.IsVerified == isVerified.Value);
        }

        // Get total count for pagination
        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        // Apply pagination and get results
        var reviews = await query
            .OrderByDescending(r => r.ReviewDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // Map to view models with additional information
        var reviewViewModels = new List<ReviewViewModel>();
        foreach (var review in reviews)
        {
            var reviewViewModel = _mapper.Map<ReviewViewModel>(review);
            
            // Get product name
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == review.ProductId);
            if (product != null)
            {
                reviewViewModel.ProductName = product.Name;
            }
            
            // Get user name
            reviewViewModel.UserName = $"{review.User.FName} {review.User.LName}";
            
            reviewViewModels.Add(reviewViewModel);
        }

        return GenericResponse<IEnumerable<ReviewViewModel>>.SuccessResponse(
            reviewViewModels, 
            "Reviews retrieved successfully",
            totalCount,
            pageNumber,
            totalPages);
    }

    public async Task<GenericResponse<ReviewViewModel>> GetReviewByIdAsync(int id)
    {
        var review = await _dbContext.Reviews
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (review == null)
        {
            return GenericResponse<ReviewViewModel>.FailureResponse($"Review with ID {id} not found");
        }

        var reviewViewModel = _mapper.Map<ReviewViewModel>(review);
        
        // Get product name
        var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == review.ProductId);
        if (product != null)
        {
            reviewViewModel.ProductName = product.Name;
        }
        
        // Set user name
        reviewViewModel.UserName = $"{review.User.FName} {review.User.LName}";

        return GenericResponse<ReviewViewModel>.SuccessResponse(reviewViewModel, "Review retrieved successfully");
    }

    public async Task<GenericResponse<ReviewViewModel>> CreateReviewAsync(CreateReviewViewModel model, string userId)
    {
        // Validate product exists
        var product = await _dbContext.Products.FindAsync(model.ProductId);
        if (product == null)
        {
            return GenericResponse<ReviewViewModel>.FailureResponse($"Product with ID {model.ProductId} not found");
        }
        
        // Validate user exists
        var user = await _dbContext.Users.FindAsync(userId);
        if (user == null)
        {
            return GenericResponse<ReviewViewModel>.FailureResponse("User not found");
        }
        
        // Check if user has already reviewed this product
        var existingReview = await _dbContext.Reviews
            .FirstOrDefaultAsync(r => r.ProductId == model.ProductId && r.UserId == userId);
            
        if (existingReview != null)
        {
            return GenericResponse<ReviewViewModel>.FailureResponse("You have already reviewed this product");
        }

        // Create and save the review
        var review = new Review
        {
            ProductId = model.ProductId,
            UserId = userId,
            Rating = model.Rating,
            Comment = model.Comment,
            ReviewDate = DateTime.UtcNow,
            IsVerified = false // Reviews start as unverified by default
        };

        _dbContext.Reviews.Add(review);
        await _dbContext.SaveChangesAsync();
        
        // Update product review metrics
        await _productService.UpdateProductReviewMetricsAsync(model.ProductId, model.Rating, true);

        // Return the created review
        var reviewViewModel = _mapper.Map<ReviewViewModel>(review);
        reviewViewModel.UserName = $"{user.FName} {user.LName}";
        reviewViewModel.ProductName = product.Name;

        return GenericResponse<ReviewViewModel>.SuccessResponse(reviewViewModel, "Review created successfully");
    }

    public async Task<GenericResponse<ReviewViewModel>> UpdateReviewAsync(UpdateReviewViewModel model, string userId)
    {
        var review = await _dbContext.Reviews
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == model.Id);

        if (review == null)
        {
            return GenericResponse<ReviewViewModel>.FailureResponse($"Review with ID {model.Id} not found");
        }

        // Check if the user owns the review or is an admin
        if (review.UserId != userId && !await IsUserAdminAsync(userId))
        {
            return GenericResponse<ReviewViewModel>.FailureResponse("You don't have permission to update this review");
        }

        // Store the previous rating to update product metrics correctly
        var previousRating = review.Rating;
        
        // Update review properties
        review.Rating = model.Rating;
        review.Comment = model.Comment;
        
        // Only admins can change the verified status
        if (await IsUserAdminAsync(userId))
        {
            review.IsVerified = model.IsVerified;
        }

        _dbContext.Reviews.Update(review);
        await _dbContext.SaveChangesAsync();
        
        // Update product review metrics - remove old rating and add new one
        await _productService.UpdateProductReviewMetricsAsync(review.ProductId, previousRating, false);
        await _productService.UpdateProductReviewMetricsAsync(review.ProductId, model.Rating, true);

        // Return the updated review
        var reviewViewModel = _mapper.Map<ReviewViewModel>(review);
        
        // Get product name
        var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == review.ProductId);
        if (product != null)
        {
            reviewViewModel.ProductName = product.Name;
        }
        
        // Set user name
        reviewViewModel.UserName = $"{review.User.FName} {review.User.LName}";

        return GenericResponse<ReviewViewModel>.SuccessResponse(reviewViewModel, "Review updated successfully");
    }

    public async Task<GenericResponse<bool>> DeleteReviewAsync(int id, string userId)
    {
        var review = await _dbContext.Reviews.FindAsync(id);
        if (review == null)
        {
            return GenericResponse<bool>.FailureResponse($"Review with ID {id} not found");
        }

        // Check if the user owns the review or is an admin
        if (review.UserId != userId && !await IsUserAdminAsync(userId))
        {
            return GenericResponse<bool>.FailureResponse("You don't have permission to delete this review");
        }
        
        // Store the rating and productId before deletion to update metrics
        var rating = review.Rating;
        var productId = review.ProductId;

        _dbContext.Reviews.Remove(review);
        await _dbContext.SaveChangesAsync();
        
        // Update product review metrics
        await _productService.UpdateProductReviewMetricsAsync(productId, rating, false);

        return GenericResponse<bool>.SuccessResponse(true, "Review deleted successfully");
    }

    public async Task<GenericResponse<bool>> VerifyReviewAsync(int id)
    {
        var review = await _dbContext.Reviews.FindAsync(id);
        if (review == null)
        {
            return GenericResponse<bool>.FailureResponse($"Review with ID {id} not found");
        }

        review.IsVerified = true;
        _dbContext.Reviews.Update(review);
        await _dbContext.SaveChangesAsync();

        return GenericResponse<bool>.SuccessResponse(true, "Review verified successfully");
    }
    
    private async Task<bool> IsUserAdminAsync(string userId)
    {
        var user = await _dbContext.Users.FindAsync(userId);
        return user?.Role == UserRole.Admin; 
    }
}
