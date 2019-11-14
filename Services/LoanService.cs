using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLoan.Data;
using BookLoan.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookLoan.Services
{
    public class LoanService: ILoanService
    {
        ApplicationDbContext _db;

        public LoanService(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: LoanViewModels/GetBookLoanStatus/5
        public async Task<BookLoan.Models.BookStatusViewModel> GetBookLoanStatus(int? bookid)
        {
            BookLoan.Models.BookStatusViewModel bsvm = new Models.BookStatusViewModel() { Status = "N/A" };

            List<BookLoan.Models.LoanViewModel> loans = await _db.Loans.
                Where(m => m.BookID == bookid).ToListAsync();

            if (loans.Count() == 0)
                bsvm.Status = "Available";

            foreach (BookLoan.Models.LoanViewModel rec in loans.OrderByDescending(a => a.DateReturn))
            {
                bool foundState = false;

                bsvm.DateLoaned = rec.DateLoaned;
                bsvm.DateReturn = rec.DateReturn;
                bsvm.DateDue = rec.DateDue;

                if (DateTime.Now >= rec.DateReturn)
                {
                    bsvm.Status = "Available";
                    foundState = true;
                }
                if (DateTime.Now <= rec.DateDue)
                {
                    bsvm.Status = "On Loan";
                    if ((DateTime.Now > rec.DateReturn) && (rec.DateReturn.Year != 1))
                        bsvm.Status = "Available";
                    foundState = true;
                }
                if ((DateTime.Now > rec.DateDue) && (rec.DateDue.Year != 1))
                {
                    bsvm.Status = "Overdue";
                    foundState = true;
                }
                if (foundState)
                    break;
            }
            return bsvm;
        }

    }
}
