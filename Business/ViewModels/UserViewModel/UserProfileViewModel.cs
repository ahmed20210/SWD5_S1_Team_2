using System.ComponentModel.DataAnnotations;

namespace Business.ViewModels.UserViewModel
{
    public class UserProfileViewModel
    {
        public string Id { get; set; }
        
        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
        public string FName { get; set; }
        
        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
        public string LName { get; set; }
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Phone number is required")]
        [Display(Name = "Phone Number")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }
    }
}
