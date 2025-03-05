namespace Domain.Entities;

public class ProductImage
{
    public int Id { get; set; }
    public string ImageURL { get; set; }
    public string Data { get; set; }
    public bool IsMain { get; set; }
    public DateTime CreatedAt { get; set; }

    [ForeignKey("product")]
    public int ProductId { get; set; }
    public Product product { get; set; }


}