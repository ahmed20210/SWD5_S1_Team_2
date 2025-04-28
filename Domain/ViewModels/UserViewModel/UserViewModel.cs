namespace Domain.ViewModels.UserViewModel;

public class UserViewModel
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsAgree { get; set; }

    public List<string> Roles { get; set; } = new List<string>();

}
