using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLoan.Services
{
    public interface IPasswordHashService
    {
        string HashPassword(string password);
    }
}
