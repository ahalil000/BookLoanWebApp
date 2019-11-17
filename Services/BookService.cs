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

        /// <summary>
        /// GetBooks()
        /// </summary>
        /// <returns></returns>
        public async Task<List<BookViewModel>> GetBooks()
        {
            return await _db.Books.ToListAsync();
        }


        /// <summary>
        /// GetBooksFilter()
        /// </summary>
        /// <returns></returns>
        public async Task<List<BookViewModel>> GetBooksFilter(string filter)
        {
            return await _db.Books.Where(b => b.Title.Contains(filter)).ToListAsync();
        }


        // GET: LoanViewModels/GetBookLoanStatus/5

        /// <summary>
        /// GetBook()
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BookViewModel> GetBook(int id)
        {
            BookLoan.Models.BookViewModel book = await _db.Books.
                Where(m => m.ID == id).SingleOrDefaultAsync();
            if (book != null)
                return book;
            return null;
        }

        /// <summary>
        /// SaveBook()
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task SaveBook(BookViewModel vm)
        { 
            _db.Add(vm);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// UpdateBook()
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task<BookViewModel> UpdateBook(int Id, BookViewModel vm)
        {
            BookViewModel book = await _db.Books.Where(a => a.ID == Id).SingleOrDefaultAsync();
            if (book != null)
            {
                book.Title = vm.Title;
                book.Author = vm.Author;
                book.Edition = vm.Edition;
                book.Genre = vm.Genre;
                book.ISBN = vm.ISBN;
                book.Location = vm.Location;
                book.YearPublished = vm.YearPublished;
                book.DateUpdated = DateTime.Now;
                _db.Update(book);
                await _db.SaveChangesAsync();
            }
            return book;
        }
    }
}
