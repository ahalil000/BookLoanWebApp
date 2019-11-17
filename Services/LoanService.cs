using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using BookLoan.Data;
using BookLoan.Domain;
using Microsoft.EntityFrameworkCore;
using BookLoan.Models;
using BookLoan.Services;


namespace BookLoan.Services
{
    public class LoanService: ILoanService
    {
        ApplicationDbContext _db;
        IBookService _bookService;
        
        public LoanService(ApplicationDbContext db, IBookService bookService)
        {
            _db = db;
            _bookService = bookService;
        }

        /// <summary>
        /// GetLoan()
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<LoanViewModel> GetLoan(int id)
        {
            var loanViewModel = await _db.Loans
                .Include(l => l.Book)
                .SingleOrDefaultAsync(m => m.ID == id);
            return loanViewModel;
        }


        /// <summary>
        /// SaveLoan()
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task SaveLoan(LoanViewModel vm)
        {
            _db.Add(vm);
            await _db.SaveChangesAsync();
        }


        /// <summary>
        /// UpdateLoan()
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task ReturnLoan(LoanViewModel vm)
        {
            vm.DateReturn = DateTime.Now;
            _db.Update(vm);
            await _db.SaveChangesAsync();
        }


        /// <summary>
        /// UpdateLoan()
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task UpdateLoan(LoanViewModel vm)
        {
            _db.Update(vm);
            await _db.SaveChangesAsync();
        }


        /// <summary>
        /// CreateNewBookLoan()
        /// </summary>
        /// <returns></returns>
        public LoanViewModel CreateNewBookLoan(int bookid)
        {
            LoanViewModel lvm = new LoanViewModel();
            lvm.DateLoaned = DateTime.Now;
            lvm.DateReturn = new DateTime(1, 1, 1); // fix 16/11/2019
            lvm.DateCreated = DateTime.Now;
            lvm.DateDue = DateTime.Now;
            lvm.OnShelf = true;
            lvm.BookID = bookid;
            return lvm;
        }


        /// <summary>
        /// GetLoan()
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<LoanViewModel>> GetBookLoans(int bookid)
        {
            var loans = await _db.Loans
                //.Include(l => l.Book)
                .Where(m => m.BookID == bookid).ToListAsync();
            return loans;
        }


        /// <summary>
        /// Return Book() - Return book from loan
        /// </summary>
        /// <param name="bookid"></param>
        /// <returns></returns>
        public async Task<(BookLoan.Models.LoanViewModel, BookLoan.Models.BookViewModel)> GetReturnLoan(int bookid)
        {

            LoanViewModel lvm = new LoanViewModel();
            BookViewModel bvm = new BookViewModel();

            var book = await _bookService.GetBook(bookid);

            if (book == null)
            {
                throw new Exception(String.Format("Book {0} cannot be found.", bookid));
            }

            var bookloans = await GetBookLoans(bookid);

            if (bookloans == null)
            {
                throw new Exception(String.Format("Loans for Book {0} cannot be found.", bookid));
            }

            // Get latest loan for the book. 
            foreach (LoanViewModel item in bookloans.OrderByDescending(o => o.DateReturn))
            {
                lvm.ID = item.ID;
                lvm.DateLoaned = item.DateLoaned;
                lvm.DateDue = item.DateDue;
                lvm.DateReturn = DateTime.Now;
                lvm.DateUpdated = DateTime.Now;
                lvm.OnShelf = false;
                lvm.LoanedBy = "";
                lvm.BookID = item.BookID;
                bvm.Title = book.Title;
                bvm.Author = book.Author;
                break;
            }

            return (lvm, bvm);
        }


        // GET: LoanViewModels/GetBookLoanStatus/5
        public async Task<BookLoan.Models.BookStatusViewModel> GetBookLoanStatus(int bookid)
        {
            BookLoan.Models.BookStatusViewModel bsvm = new Models.BookStatusViewModel() { Status = "N/A" };

            List<BookLoan.Models.LoanViewModel> loans = await _db.Loans.
                Where(m => m.BookID == bookid).ToListAsync();

            if (loans.Count() == 0)
                bsvm.Status = "Available";

            foreach (BookLoan.Models.LoanViewModel rec in loans.OrderByDescending(a => a.DateLoaned))
            {
                bool foundState = false;

                bsvm.DateLoaned = rec.DateLoaned;
                bsvm.DateReturn = rec.DateReturn;
                bsvm.DateDue = rec.DateDue;

                if (DateTime.Now >= rec.DateReturn)
                {
                    bsvm.Status = "Available";
                    foundState = true;
                }
                if (DateTime.Now <= rec.DateDue)
                {
                    bsvm.Status = "On Loan";
                    if ((DateTime.Now > rec.DateReturn) && (rec.DateReturn.Year != 1))
                        bsvm.Status = "Available";
                    foundState = true;
                }
                if ((DateTime.Now > rec.DateDue) && (rec.DateDue.Year != 1))
                {
                    bsvm.Status = "Overdue";
                    foundState = true;
                }
                if (foundState)
                    break;
            }
            return bsvm;
        }

    }
}
