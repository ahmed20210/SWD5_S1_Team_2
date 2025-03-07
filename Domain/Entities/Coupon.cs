

namespace Domain.Entities
{
   public class Coupon
    {
        
        public int Id { get; set; }  

       
        public string Code { get; set; }  

       
        public CouponType Type { get; set; }  

      
        public decimal Value { get; set; }  

     
        public int UsageLimit { get; set; }  

        public int UsageCount { get; set; } = 0;  

        public decimal? MaxDiscount { get; set; }  

        public decimal MinOrderValue { get; set; }  

       
        public DateTime StartDate { get; set; }  

       
        public DateTime ExpiryDate { get; set; }  

        public CouponStatus Status { get; set; }
        public Booster Booster { get; set; }
    }











}
}
