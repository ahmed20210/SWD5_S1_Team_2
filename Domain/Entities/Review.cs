using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Review
{
    public int Id { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }
    [MaxLength(500)]
    public string Comment { get; set; }
    
    public bool IsVerified { get; set; } = false;
    public DateTime ReviewDate { get; set; }

    public string UserId { get; set; }
    public User User { get; set; }

    public int ProductId { get; set; }


}
