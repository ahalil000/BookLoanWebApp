using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLoan.Services
{
    public interface IReportService
    {
        Task<List<BookLoan.Models.BookStatusViewModel>> OnLoanReport(string currentuser = null);
        Task<List<BookLoan.Models.BookStatusViewModel>> MyOnLoanReport();
    }
}
