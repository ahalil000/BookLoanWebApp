using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookLoan.Data;
using BookLoan.Models;

namespace BookLoan.Views.Loan
{
    public class EditModel : PageModel
    {
        private readonly BookLoan.Data.ApplicationDbContext _context;

        public EditModel(BookLoan.Data.ApplicationDbContext context)
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
           ViewData["BookID"] = new SelectList(_context.Books, "ID", "Author");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(LoanViewModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoanViewModelExists(LoanViewModel.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool LoanViewModelExists(int id)
        {
            return _context.Loans.Any(e => e.ID == id);
        }
    }
}
