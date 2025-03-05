namespace Domain.Entities;



public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public ProductStatus Status { get; set; }
    public int CountOfViews { get; set; }
    public int CountOfPurchase { get; set; }
    public int CountOfReviews { get; set;}

   
   

    [ForeignKey("category")]
    public int CategoryId { get; set; }
    public Category category { get; set; }

    public ICollection<ProductImage> ProductImages { get; set; }
    public ICollection<Review> Reviews { get; set; }

    public ICollection<FavouriteList> favouritelists { get; set; }


}