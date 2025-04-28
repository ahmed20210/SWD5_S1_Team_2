

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class User : IdentityUser
{

   
     [Required][MaxLength(20)]
    public string FName { get; set; }
    [Required][MaxLength(20)]
    public string LName { get; set; }

    [NotMapped]
    public string FullName => FName + " " + LName;
    public UserRole Role { get; set; }

    public bool IsAgree { get; set; }
    public bool IsVerified { get; set; } = false;
    public int VerificationCode { get; set; }
    
    
    public int MainAddressId { get; set; }
    public Address MainAddress { get; set; }
    
    public ICollection<Address> Addresses { get; set; }
   
    public ICollection<Notification> Notifications { get; set; }
    public ICollection<FavouriteList> FavouriteLists { get; set; }
    
    public ICollection<Order> Orders { get; set; }
    
    public ICollection<Log> Logs { get; set; }
    
    
}
