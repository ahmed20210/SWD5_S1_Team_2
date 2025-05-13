using System.ComponentModel.DataAnnotations;
namespace Domain.Entities;

public class OrderItem
{
   
    [ Required]
    public int Quantity { get; set; }
    
    [Required]
    public decimal TotalAmount { get; set; }
    [Required]
    public decimal UnitPrice { get; set; }

    public int? DiscountId { get; set; }
    public Discount? Discount { get; set; }
    [Required]
   public int OrderId { get; set; } 
    
    public int ProductId { get; set; } 
    public Product Product { get; set; }  
}