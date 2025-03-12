using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Booster
{


    public int Id { get; set; }
    
    public string ImagePath { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public BoosterTypes BoosterType { get; set; }
    public string BoosterURL { get; set; }
    [ForeignKey("CouponId")] 
    public int? CouponId { get; set; }
    public Coupon Coupon { get; set; }


    public Discount Discount { get; set; } = null!;
    [ForeignKey("OrderId")]
    public int? OrderId { get; set; }
    public Order Order { get; set; }


}
