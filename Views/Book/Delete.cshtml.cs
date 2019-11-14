using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BookLoan.Data;
using BookLoan.Models;

namespace BookLoan.Views.Book
{
    public class DeleteModel : PageModel
    {
        private readonly BookLoan.Data.ApplicationDbContext _context;

        public DeleteModel(BookLoan.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BookViewModel BookViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BookViewModel = await _context.Books.SingleOrDefaultAsync(m => m.ID == id);

            if (BookViewModel == null)
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

            BookViewModel = await _context.Books.FindAsync(id);

            if (BookViewModel != null)
            {
                _context.Books.Remove(BookViewModel);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
