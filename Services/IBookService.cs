using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLoan.Services
{
    public interface IBookService
    {
        Task<BookLoan.Models.BookViewModel> GetBook(int id);
    }
}
