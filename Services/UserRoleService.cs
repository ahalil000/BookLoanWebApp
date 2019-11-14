using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BookLoan.Data;
using BookLoan.Models;

namespace BookLoan.Services
{

    public class UserRoleService: IUserRoleService
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> userManager;

        public UserRoleService(ApplicationDbContext _db, UserManager<ApplicationUser> _userManager)
        {
            db = _db;
            userManager = _userManager;
        }

        public async Task<List<string>> GetUserRoles(string userName)
        {
            var user = await userManager.FindByEmailAsync(userName);
            IList<string> roles = await userManager.GetRolesAsync(user);
            if (roles != null)
                return roles.ToList();
            return new List<string>();
        }

        public async Task<bool> IsUserInRole(string userName, string role)
        {
            var user = await userManager.FindByEmailAsync(userName);
            bool isInRole = await userManager.IsInRoleAsync(user, role);
            return isInRole;
        }

        public async Task AddUserToRole(string userName, string role)
        {
            var user = await userManager.FindByEmailAsync(userName);
            bool isInRole = await userManager.IsInRoleAsync(user, role);
            if (!isInRole)
            {
                await userManager.AddToRoleAsync(user, role.ToUpper());
            }
        }
    }
}
