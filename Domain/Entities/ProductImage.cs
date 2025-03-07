using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class ProductImage
{
    public int Id { get; set; }
    public string ImageURL { get; set; }
    public string Data { get; set; }
    public bool IsMain { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public int ProductId { get; set; }
    [ForeignKey("Product")]
    public Product Product { get; set; }


}