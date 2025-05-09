using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;



public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    [Required]
    public decimal Price { get; set; }

    [MaxLength(1000)]
    public string Description { get; set; }
    
    public int Stock { get; set; }
  
    public double AverageReviewScore { get; set; }
    
    public ProductStatus Status { get; set; }
    public int NoOfViews { get; set; } = 0;
    public int NoOfPurchase { get; set; } = 0;
    public int NoOfReviews { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(500)]
   public string? ImageUrl { get; set; } 
    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public ICollection<ProductImage> Images { get; set; }
    
    public ICollection<Review> Reviews { get; set; }
    
    // feature For Admin
    public ICollection<OrderItem> OrderItems { get; set; }
    
    
    public int? DiscountId { get; set; }  

    public Discount? Discount { get; set; }
    
    public ICollection<Discount>? PastDiscountList { get; set; }
    
    



}
