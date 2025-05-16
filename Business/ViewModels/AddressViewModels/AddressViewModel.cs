using System.ComponentModel.DataAnnotations;

namespace Business.ViewModels.AddressViewModels;

public class AddressViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "City is required")]
    [MaxLength(100)]
    public string City { get; set; }
    
    [Required(ErrorMessage = "Zip Code is required")]
    [MaxLength(10)]
    public string ZipCode { get; set; }
    
    [Required(ErrorMessage = "State is required")]
    [MaxLength(100)]
    public string State { get; set; }
    
    [MaxLength(200)]
    public string? Street { get; set; }
    
    [Required(ErrorMessage = "Country is required")]
    [MaxLength(100)]
    public string Country { get; set; }
    
    public bool IsMainAddress { get; set; }
}

public class CreateAddressViewModel
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

public class UpdateAddressViewModel
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
