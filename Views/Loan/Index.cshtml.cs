using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BookLoan.Data;
using BookLoan.Models;

namespace BookLoan.Views.Loan
{
    public class IndexModel : PageModel
    {
        private readonly BookLoan.Data.ApplicationDbContext _context;

        public IndexModel(BookLoan.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<LoanViewModel> LoanViewModel { get;set; }

        public async Task OnGetAsync()
        {
            LoanViewModel = await _context.Loans
                .Include(l => l.Book).ToListAsync();
        }
    }
}
