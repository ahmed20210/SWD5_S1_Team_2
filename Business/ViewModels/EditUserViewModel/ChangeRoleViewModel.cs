using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Business.ViewModels.EditUserViewModel
{
    public class ChangeRoleViewModel
    {
        public string Id { get; set; }
        
        public string FullName { get; set; }
        
        public string Email { get; set; }
        
        [Display(Name = "Current Role")]
        public UserRole CurrentRole { get; set; }
        
        [Display(Name = "New Role")]
        public UserRole NewRole { get; set; }
    }
}
