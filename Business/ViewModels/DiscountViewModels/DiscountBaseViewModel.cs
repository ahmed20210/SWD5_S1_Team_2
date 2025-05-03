namespace Business.ViewModels.DiscountViewModels;

public abstract class DiscountBaseViewModel
{
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int ProductId { get; set; }
    
}
