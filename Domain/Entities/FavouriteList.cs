using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class FavouriteList
{
    public int Id { get; set; }

    public int ProductID { get; set; }
    public DateTime CreatedAt { get; set; }

    [ForeignKey("UserId")]
    public int UserId { get; set; }
    public User user { get; set; }

    [ForeignKey("ProductId")]
    public int ProductId { get; set; }
    public Product product { get; set; }
}