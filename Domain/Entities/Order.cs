namespace Domain.Entities;

public class Order
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int? CouponId { get; set; }

    public int AddressId { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Note { get; set; }

    public OrderStatus Status { get; set; }

    public ICollection<OrderTimeLine> ordertimelines { get; set; }

}