using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Address
{
    
 public int Id { get; set; }

[Required]
[MaxLength(100)]
public string City { get; set; }

[Required]
[MaxLength(10)]
public string ZipCode { get; set; }

[Required]
[MaxLength(100)]
public string State { get; set; }


[MaxLength(200)]
public string? Street { get; set; }

[Required]
[MaxLength(100)]
public string Country { get; set; }
  
    public string UserId { get; set; }
    
}
