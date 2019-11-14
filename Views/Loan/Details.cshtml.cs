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
    public class DetailsModel : PageModel
    {
        private readonly BookLoan.Data.ApplicationDbContext _context;

        public DetailsModel(BookLoan.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public LoanViewModel LoanViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            LoanViewModel = await _context.Loans
                .Include(l => l.Book).SingleOrDefaultAsync(m => m.ID == id);

            if (LoanViewModel == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
