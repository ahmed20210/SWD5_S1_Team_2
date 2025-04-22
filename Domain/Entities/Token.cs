

using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
  public class Token
    {
       
        public int Id { get; set; }  
      
        public string UserId { get; set; }

        public User User { get; set; }

        public string? DeviceDetails { get; set; }  

       public UserRole Role { get; set; }
       [MaxLength(20)]
        public string SecretKey { get; set; } 
        
        public DateTime ExpiryDate { get; set; }  
        
    }

}

