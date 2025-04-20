using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class ProductViewModel
{
    public IEnumerable<Product> Products { get; set; } = new List<Product>();
    
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 10;
    public int TotalCount { get; set; }
    
    public string SearchTerm { get; set; } = string.Empty;
    public int? CategoryId { get; set; }
    public string OrderBy { get; set; } = "name";
    
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    
    public IEnumerable<Category> AvailableCategories { get; set; } = new List<Category>();
    
    // Helper properties for view
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
    
    public string GetSortClass(string sortField)
    {
        if (string.IsNullOrEmpty(OrderBy) && sortField == "name")
            return "sort-asc";
            
        if (OrderBy == $"{sortField}_desc")
            return "sort-desc";
            
        if (OrderBy == sortField || (sortField == "name" && string.IsNullOrEmpty(OrderBy)))
            return "sort-asc";
            
        return string.Empty;
    }
}

public class ProductCreateViewModel
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; } = string.Empty;

    [Required]
    [Range(0.01, 1000000)]
    public decimal Price { get; set; }

    [Required]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    public IEnumerable<Category> AvailableCategories { get; set; } = new List<Category>();
}