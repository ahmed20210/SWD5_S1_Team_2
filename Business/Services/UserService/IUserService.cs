using Domain.Entities;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.UserService
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(string id);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> SuspendUserAsync(string id);
        Task<bool> ActivateUserAsync(string id);
        Task<bool> ChangeUserRoleAsync(string id, UserRole newRole);
    }
}
