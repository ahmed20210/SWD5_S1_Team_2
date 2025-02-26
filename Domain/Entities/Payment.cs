
namespace Domain.Entities;

public class Payment
{
    public int Id { get; set; }
    public string TransactionID { get; set; } 
    public int OrderID { get; set; } 
    public DateTime CreatedAt { get; set; } 
    public int Amount { get; set; }  
    public PaymentStatus Status { get; set; }
    public PaymentMethods Method { get; set; }
    
}