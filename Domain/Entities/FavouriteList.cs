using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class FavouriteList
{
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
}