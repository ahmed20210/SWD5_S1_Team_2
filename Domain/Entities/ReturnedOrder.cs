namespace Domain.Entities;

public class ReturnedOrder
{
   public int Id { get; set; }
   public string OrderID { get; set; }
   public int ProductID { get; set; }
   public int Quantity { get; set; }
   public string Reason { get; set; }
   public string Description { get; set; }
   public DateTime CreatedAt { get; set; }
   public int RefundAmount { get; set; }
   public RefundStatus RefundStatus { get; set; }
   // public Order Order { get; set; }
}
