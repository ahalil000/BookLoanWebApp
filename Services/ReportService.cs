using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLoan.Data;
using BookLoan.Domain;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using BookLoan.Models;


namespace BookLoan.Services
{
    public class ReportService: IReportService
    {
        ApplicationDbContext _db;
        ILoanService _loanService;
        IBookService _bookService;
        UserManager<ApplicationUser> _userManager;
        HttpContext _context;

        public ReportService(ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IBookService bookService, 
            ILoanService loanService)
        {
            _db = db;
            _bookService = bookService;
            _loanService = loanService;
            _context = httpContextAccessor.HttpContext;
        }

        public async Task<bool> CurrentUserAnyOverdueLoans()
        {
            string curruser = _context.User.Identity.Name;
            List<BookLoan.Models.BookStatusViewModel> bookStatusViews =
                await MyOnLoanReport();
            return bookStatusViews.Any(a => a.Status == "OVERDUE");
        }


        public async Task<List<BookLoan.Models.BookStatusViewModel>> MyOnLoanReport()
        {
            string curruser = _context.User.Identity.Name; 
            return await OnLoanReport(curruser);
        }

        public async Task<List<BookLoan.Models.BookStatusViewModel>> OnLoanReport(string currentuser = null)
        {
            List<BookLoan.Models.BookStatusViewModel> loanstats = new List<Models.BookStatusViewModel>();

            //var bookloans = await _db.Loans.
            //    Where(a => a.DateDue <= DateTime.Now).
            //    ToListAsync();

            var books = await _db.Books.ToListAsync();

            foreach (Models.BookViewModel book in books)
            {
                BookLoan.Models.BookStatusViewModel bsvm = await _loanService.GetBookLoanStatus(book.ID);
                //BookLoan.Models.BookViewModel bvm = await _bookService.GetBook(book.ID);
                if (((currentuser != null) && (bsvm.Borrower == currentuser))
                        || (currentuser == null))
                {
                    loanstats.Add(new Models.BookStatusViewModel()
                    {
                        ID = book.ID,
                        Author = book.Author,
                        Title = book.Title,
                        Genre = book.Genre,
                        ISBN = book.ISBN,
                        Edition = book.Edition,
                        Location = book.Location,
                        YearPublished = book.YearPublished,
                        OnShelf = bsvm.OnShelf,
                        DateLoaned = bsvm.DateLoaned,
                        DateReturn = bsvm.DateReturn,
                        DateDue = bsvm.DateDue,
                        Status = bsvm.Status,
                        Borrower = bsvm.Borrower
                    });
                }
            }
            return loanstats;
        }


        //public async Task<List<BookLoan.Models.BookStatusViewModel>> OverdueReport()
        //{
        //    List<BookLoan.Models.BookStatusViewModel> loanstats = new List<Models.BookStatusViewModel>();

        //    var bookloans = await _db.Loans.
        //        Where(a => a.DateDue <= DateTime.Now).
        //        ToListAsync();
        //    foreach (Models.LoanViewModel loan in bookloans)
        //    {
        //        BookLoan.Models.BookStatusViewModel bsvm = await _loanService.GetBookLoanStatus(loan.BookID);
        //        BookLoan.Models.BookViewModel bvm = await _bookService.GetBook(loan.BookID);
        //        loanstats.Add(new Models.BookStatusViewModel()
        //        {
        //            ID = bvm.ID,
        //            Author = bvm.Author,
        //            Title = bvm.Title,
        //            Genre = bvm.Genre,
        //            ISBN = bvm.ISBN,
        //            Edition = bvm.Edition,
        //            Location = bvm.Location,
        //            YearPublished = bvm.YearPublished,
        //            OnShelf = bsvm.OnShelf,
        //            DateLoaned = bsvm.DateLoaned,
        //            DateReturn = bsvm.DateReturn,
        //            Status = bsvm.Status
        //        });
        //    }
        //    return loanstats;
        //}



        // GET: LoanViewModels/GetBookLoanStatus/5
        public BookLoan.Models.BookStatusViewModel GetBookLoanStatus(int? bookid)
        {
            BookLoan.Models.BookStatusViewModel bsvm = new Models.BookStatusViewModel() { Status = "N/A" };

            List<BookLoan.Models.LoanViewModel> loans = _db.Loans.
                Where(m => m.BookID == bookid).ToList();

            if (loans.Count() == 0)
                bsvm.Status = "Available";

            foreach (BookLoan.Models.LoanViewModel rec in loans.OrderByDescending(a => a.DateReturn))
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
                    if ((DateTime.Now > rec.DateReturn) && (rec.DateReturn.Year != 1) )
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
