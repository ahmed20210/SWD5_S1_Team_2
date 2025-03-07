using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class OrderTimeLine

{
    public int Id { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime ChangedAt { get; set; }
    public string Description { get; set; }
    [ForeignKey("OrderId")] 
    public int OrderId { get; set; }
    public Order Order { get; set; }

}