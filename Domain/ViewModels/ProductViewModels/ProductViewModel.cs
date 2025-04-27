using Domain.DTOs.ProductDTOs;

namespace Domain.ViewModels.ProductViewModels;


public class ProductViewModel : ProductBaseViewModel
{
   
}

public class CreateProductViewModel : ProductBaseViewModel
{
    
}

public class UpdateProductViewModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal? Price { get; set; }
}

public class GetAllProductsViewModel : ProductBaseViewModel
{
    public int Id { get; set; }
}

public class GetProductViewModel : ProductBaseViewModel
{
    
}
