using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Address
{
    
    public int Id { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public string State { get; set; }
    public string Street { get; set; }
    public string Country { get; set; }
    [ForeignKey("user")]
    public int UserId { get; set; }
    public User User { get; set; }
}