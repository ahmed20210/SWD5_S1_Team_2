using System.ComponentModel.DataAnnotations;

namespace Business.ViewModels.AddressViewModels;

public class AddressDto
{
    public int Id { get; set; }
    
    public string City { get; set; }
    
    public string ZipCode { get; set; }
    
    public string State { get; set; }
    
    public string? Street { get; set; }
    
    public string Country { get; set; }
    
    public bool IsMainAddress { get; set; }
}

public class CreateAddressDto
{
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
}

public class UpdateAddressDto
{
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
}
