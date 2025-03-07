using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Booster
{


    public int ID { get; set; }


    public string ImagePath { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public BoosterTypes BoosterType { get; set; }
    public string BoosterURL { get; set; }

    [ForeignKey("couponId")]
    public int? couponId { get; set; }
    public Coupon coupon { get; set; }



    [ForeignKey("DiscoutID")]
    public int? DiscoutID { get; set; }
    public Discount discount { get; set; }

    // Not clear
    [ForeignKey("OrderID")]
    public int? OrderID { get; set; }
    public Order order { get; set; }


}
