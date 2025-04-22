using Domain.DTOs.ProductDTOs;

namespace Domain.DTOs.DiscountDTOs;

public class DiscountDTO : DiscountBaseDto
{
   
}

public class CreateDiscountDTO : DiscountBaseDto
{
    
    public ProductBaseDto? Product { get; set; } 
}

public class UpdateDiscountDTO  
{
    public int Id { get; set; }
    public ProductBaseDto? Product { get; set; }
    public int? ProductId { get; set; }
    public decimal? Amount { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class GetAllDiscountsDTO : DiscountBaseDto
{
    public int Id { get; set; }
    public ProductBaseDto? Product { get; set; }

}

public class GetDiscountDTO : DiscountBaseDto
{
    
}
