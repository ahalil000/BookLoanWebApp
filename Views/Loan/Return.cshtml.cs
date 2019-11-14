using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookLoan.Models;

namespace BookLoan.Views.Loan
{
    public class ReturnModel : PageModel
    {
        private readonly BookLoan.Data.ApplicationDbContext _context;

        public ReturnModel(BookLoan.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LoanViewModel LoanViewModel { get; set; }
        public BookViewModel BookViewModel { get; set; }

        public void OnGet()
        {

        }
    }
}