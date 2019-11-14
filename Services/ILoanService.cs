using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLoan.Services
{
    public interface ILoanService
    {
        Task<BookLoan.Models.BookStatusViewModel> GetBookLoanStatus(int? bookid);
    }
}
