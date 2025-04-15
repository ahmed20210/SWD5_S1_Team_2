using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Banner
{


    public int Id { get; set; }
    [Required][MaxLength(200)]
    public string ImagePath { get; set; }
    [MaxLength(20)]
    public string Title { get; set; }
    [MaxLength(200)]
    public string Description { get; set; }
    public BoosterTypes BoosterType { get; set; }
    [MaxLength(200)]
    public string? URL { get; set; }
    public int? CouponId { get; set; }
    public Coupon? Coupon { get; set; }
    
    public int? ProductId { get; set; }
    public Product? Product { get; set; }
   
    
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiredAt { get; set; }
    public bool IsActive { get; set; }
    


}
