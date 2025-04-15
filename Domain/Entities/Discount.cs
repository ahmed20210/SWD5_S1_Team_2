using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Discount
{
    public int Id { get; set; }
    [Range(0, 100)]
    public decimal Amount { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public int ProductId { get; set; }
    public Product Product { get; set; }
    
  
    


}