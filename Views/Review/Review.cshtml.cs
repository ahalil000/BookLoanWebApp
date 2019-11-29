using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookLoan.Models;

namespace BookLoan.Views.Review
{
    public class ReviewModel : PageModel
    {
        private readonly BookLoan.Data.ApplicationDbContext _context;

        public ReviewModel(BookLoan.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BookLoan.Models.ReviewViewModel ReviewViewModel { get; set; }

        public void OnGet()
        {

        }
    }
}