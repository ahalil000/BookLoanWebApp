using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLoan.Data;
using BookLoan.Domain;
using BookLoan.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLoan.Services
{
    public class BookService: IBookService
    {
        ApplicationDbContext _db;

        public BookService(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: LoanViewModels/GetBookLoanStatus/5
        public async Task<BookViewModel> GetBook(int id)
        {
            BookLoan.Models.BookViewModel book = await _db.Books.
                Where(m => m.ID == id).SingleOrDefaultAsync();

            return book;
        }
    }
}
