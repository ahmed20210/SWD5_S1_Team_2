
namespace Domain.Entities;

public class Payment
{
    public int Id { get; set; }
    public string TransactionId { get; set; } 
    public int OrderId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public decimal Amount { get; set; }  
    public PaymentStatus Status { get; set; }
    public PaymentMethods Method { get; set; }
    
}
