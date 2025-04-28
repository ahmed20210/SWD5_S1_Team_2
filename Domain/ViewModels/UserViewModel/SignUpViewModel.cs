using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModels.UserViewModel
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "UserName is required")]
            public string FName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        public string LName { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Phone number must be between 10 and 15 digits")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress (ErrorMessage = "Invalid Email")]
        
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(5,ErrorMessage = "Minimum Password Length is 5")]
        [DataType(DataType.Password)]
        
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Confirm Password does not match Password")]
        public string ConfirmPassword { get; set; }

        public bool IsAgree { get; set; }
    }
}
