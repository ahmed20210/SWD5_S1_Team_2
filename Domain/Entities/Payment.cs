
namespace Domain.Entities;

public class Payment
{
    public int Id { get; set; }
    public string TransactionId { get; set; } 
    public int OrderId { get; set; } 
    public DateTime CreatedAt { get; set; } 
    public int Amount { get; set; }  
    public PaymentStatus Status { get; set; }
    public PaymentMethods Method { get; set; }
    
}