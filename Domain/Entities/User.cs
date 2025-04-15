using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class User
{

    public int Id { get; set; }
     [Required][MaxLength(20)]
    public string FName { get; set; }
    [Required][MaxLength(20)]
    public string LName { get; set; }

    [NotMapped]
    public string FullName => FName + " " + LName;
    
    [Required][MaxLength(50)]
    public string Password { get; set; }
    
    [Required][MaxLength(50)]
    public string Email { get; set; }
    [MaxLength(20)]
    public string Phone { get; set; }
    
    public UserRole Role { get; set; }
    public bool IsVerified { get; set; }
    public int VerificationCode { get; set; }
    
    
    public int MainAddressId { get; set; }
    public Address MainAddress { get; set; }
    
    public ICollection<Address> Addresses { get; set; }
   
    public ICollection<Notification> Notifications { get; set; }
    public ICollection<FavouriteList> FavouriteLists { get; set; }
    
    public ICollection<Order> Orders { get; set; }
    
    public ICollection<Log> Logs { get; set; }
    
    
}