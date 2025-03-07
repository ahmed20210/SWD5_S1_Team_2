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


    [ForeignKey("DiscoutId")]
    public int? DiscoutId { get; set; }
    public Discount discount { get; set; }
    [ForeignKey("OrderId")]
    public int? OrderId { get; set; }
    public Order Order { get; set; }


}
