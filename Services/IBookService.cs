using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLoan.Models;

namespace BookLoan.Services
{
    public interface IBookService
    {
        Task<List<BookViewModel>> GetBooks();
        Task<List<BookViewModel>> GetBooksFilter(string filter);
        Task<BookLoan.Models.BookViewModel> GetBook(int id);
        Task SaveBook(BookViewModel vm);
        Task<BookViewModel> UpdateBook(int id, BookViewModel vm);
    }
}
