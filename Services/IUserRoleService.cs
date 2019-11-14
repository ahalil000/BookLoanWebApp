using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLoan.Services
{
    public interface IUserRoleService
    {
        Task<List<string>> GetUserRoles(string userName);
        Task<bool> IsUserInRole(string userName, string role);
        Task AddUserToRole(string userName, string role);
    }
}
