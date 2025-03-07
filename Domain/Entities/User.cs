using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class User
{

    public int Id { get; set; }

    public string FName { get; set; }
    public string LName { get; set; }

    [NotMapped]
    public string FullName => FName + " " + LName;
    public string Password { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public UserRole Role { get; set; }
    public bool IsVerified { get; set; }
    public int VerificationCode { get; set; }
    
    public ICollection<Address> Addresses { get; set; }
    [ForeignKey("MainAddressId")]
    public int MainAddressId { get; set; }
    public Address Address { get; set; }

    public ICollection<Review> Reviews { get; set; }
    public ICollection<FavouriteList> FavouriteList { get; set; }


}