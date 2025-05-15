using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Business.ViewModels.EditUserViewModel
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public bool IsVerified { get; set; }
        public UserRole Role { get; set; }
    }
}
