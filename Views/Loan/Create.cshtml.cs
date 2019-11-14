using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookLoan.Data;
using BookLoan.Models;

namespace BookLoan.Views.Loan
{
    public class CreateModel : PageModel
    {
        private readonly BookLoan.Data.ApplicationDbContext _context;

        public CreateModel(BookLoan.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["BookID"] = new SelectList(_context.Books, "ID", "Author");
            return Page();
        }

        [BindProperty]
        public LoanViewModel LoanViewModel { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Loans.Add(LoanViewModel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}