﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookLoan.Views.Dashboard
{
    public class DashboardModel : PageModel
    {
        private readonly BookLoan.Data.ApplicationDbContext _context;

        public DashboardModel(BookLoan.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {

        }
    }
}