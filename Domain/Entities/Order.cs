using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Order
{
    [Key]
    public int Id { get; set; }

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalAmount { get; set; }

    [MaxLength(500)] 
    public string? Note { get; set; }

    public OrderStatus Status { get; set; }
    [Required] 
    
    public string CustomerId { get; set; }
    public User Customer { get; set; }
    
    public int? PaymentId { get; set; }
    public Payment? Payment { get; set; }
    
    public int? CouponId { get; set; }
    public Coupon? Coupon { get; set; }
    
    public int? AddressId { get; set; }
    public Address? Address { get; set; }
    
    public ICollection<OrderTimeLine> OrderTimeLines { get; set; } = new List<OrderTimeLine>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
