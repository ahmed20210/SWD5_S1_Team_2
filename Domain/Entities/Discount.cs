namespace Domain.Entities;

public class Discount
{
    public int Id { get; set; }
    
    public int? ProductId { get; set; }
    public int? OrderId { get; set; }
    public string? Name { get; set; }

    public decimal Amount { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    public int BoosterId { get; set; }

}