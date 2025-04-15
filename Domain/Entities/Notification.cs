using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Notification
{
    public int Id { get; set; }

    [MaxLength(200)] public string? URL { get; set; }
    [MaxLength(20)] public string Title { get; set; }
    [MaxLength(200)] public string Message { get; set; }
    public bool? IsRead { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }


    public int UserId { get; set; }
    public User User { get; set; }
}