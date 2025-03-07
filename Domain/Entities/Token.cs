

namespace Domain.Entities
{
  public class Token
    {
       
        public int Id { get; set; }  
      
        public int UserId { get; set; }  
        
        public string DeviceDetails { get; set; }  

       public UserRole Role { get; set; }
       
        public string SecretKey { get; set; } 
        
        public DateTime ExpiryDate { get; set; }  
        
    }

}

