using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModels.UserViewModel
{
    public class SignUpViewModel
    {
        
        [Required(ErrorMessage = "UserName is required")]
        [Display(Name = "First Name")]
            public string FName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        public string LName { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Phone number must be between 10 and 15 digits")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress (ErrorMessage = "Invalid Email")]

        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(5,ErrorMessage = "Minimum Password Length is 5")]
    
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Confirm Password does not match Password")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please agree to the terms and conditions")]
        [Display(Name = "I agree to the terms and conditions")]
        public bool IsAgree { get; set; }
    }
}
