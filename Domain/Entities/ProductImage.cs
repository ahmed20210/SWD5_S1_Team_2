using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class ProductImage
{
    public int Id { get; set; }
    [MaxLength(200)]
    public string ImageURL { get; set; }
    
    public bool IsMain { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public int ProductId { get; set; }


}