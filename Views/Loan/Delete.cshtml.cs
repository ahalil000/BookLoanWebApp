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
    public class DeleteModel : PageModel
    {
        private readonly BookLoan.Data.ApplicationDbContext _context;

        public DeleteModel(BookLoan.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            LoanViewModel = await _context.Loans.FindAsync(id);

            if (LoanViewModel != null)
            {
                _context.Loans.Remove(LoanViewModel);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
