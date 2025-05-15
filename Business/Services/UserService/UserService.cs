using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Domain;

namespace Business.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            // Filter out admin users, only return client users
            return await _userManager.Users
                .Where(u => u.Role == Domain.UserRole.Client)
                .ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> SuspendUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return false;

            user.IsVerified = false;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> ActivateUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return false;

            user.IsVerified = true;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
        
        public async Task<bool> ChangeUserRoleAsync(string id, UserRole newRole)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return false;
            
            user.Role = newRole;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}
