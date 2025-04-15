using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;



public class Category
{
    
    public int Id { get; set; }
    [Required][MaxLength(20)]
    public string Name { get; set; }
    [MaxLength(200)]
    public string? Description { get; set; }
    [MaxLength(200)]
    public string ImagePath { get; set; }

    public ICollection<Product> Products { get; set; }

}