using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookLoan.Models;
using BookLoan.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BookLoan.Views.Book;
using BookLoan.Services;

namespace BookLoan.Controllers
{
    public class BookController : Controller
    {
        ApplicationDbContext _db;
        ILoanService _loanservice;

        public BookController(ApplicationDbContext db, ILoanService loanService)
        {
            _db = db;
            _loanservice = loanService;
        }

        // GET: Book
        public ActionResult Index()
        {
            return View();
        }

        // GET: Book/Details/5
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0)
            {
                return null;

            }
            BookStatusViewModel bvm = new BookStatusViewModel();
            var book = await _db.Books.Where(a => a.ID == id).SingleOrDefaultAsync();
            var loanStatus = await _loanservice.GetBookLoanStatus(id);
            if (book != null)
            {
                bvm.ID = book.ID;
                bvm.Title = book.Title;
                bvm.Author = book.Author;
                bvm.Genre = book.Genre;
                bvm.Location = book.Location;
                bvm.YearPublished = book.YearPublished;
                bvm.Edition = book.Edition;
                bvm.Genre = book.Genre;
                bvm.DateCreated = book.DateCreated;
                bvm.DateUpdated = book.DateUpdated;

                bvm.Status = loanStatus.Status.ToString().ToUpper();
                bvm.DateLoaned = loanStatus.DateLoaned;
                bvm.DateReturn = loanStatus.DateReturn;
            }
            BookLoan.Views.Book.DetailsModel detailsModel = new DetailsModel(_db);
            loanStatus.Status = loanStatus.Status.ToUpper();
            detailsModel.BookViewModel = bvm;
            detailsModel.BookStatusViewModel = loanStatus;

            return View(detailsModel);
        }

        // GET: Book/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                //int Id = System.Convert.ToInt32(collection["BookViewModel.ID"].ToString());
                string sTitle = collection["BookViewModel.Title"].ToString();
                string sAuthor = collection["BookViewModel.Author"].ToString();
                int iYearPublished = System.Convert.ToInt32(collection["BookViewModel.YearPublished"].ToString());
                string sGenre = collection["BookViewModel.Genre"].ToString();
                string sEdition = collection["BookViewModel.Edition"].ToString();
                string sISBN = collection["BookViewModel.ISBN"].ToString();
                string sLocation = collection["BookViewModel.Location"].ToString();

                BookViewModel book = new BookViewModel()
                {
                    Title = sTitle,
                    Author = sAuthor,
                    YearPublished = iYearPublished,
                    Genre = sGenre,
                    Edition = sEdition,
                    ISBN = sISBN,
                    Location = sLocation,
                    DateCreated = DateTime.Today
                };

                if (ModelState.IsValid)
                {
                    _db.Add(book);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Details", "Book", new { id = book.ID });
                }
                ViewData["BookID"] = new SelectList(_db.Books, "ID", "Author", book.ID);
                return View(book);
            }
            catch
            {
                return View();
            }
        }

        // GET: Book/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return null;

            }
            BookViewModel bvm = new BookViewModel();
            var book = await _db.Books.Where(a => a.ID == id).SingleOrDefaultAsync();
            if (book != null)
            {
                bvm.ID = book.ID;
                bvm.Title = book.Title;
                bvm.Author = book.Author;
                bvm.Genre = book.Genre;
                bvm.Location = book.Location;
                bvm.YearPublished = book.YearPublished;
                bvm.Edition = book.Edition;
                bvm.ISBN = book.ISBN;
                bvm.DateUpdated = DateTime.Now;
            }
            BookLoan.Views.Book.EditModel editModel = new EditModel(_db);
            editModel.BookViewModel = bvm;

            return View(editModel);
        }

        // POST: Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                int Id = System.Convert.ToInt32(collection["BookViewModel.ID"].ToString());
                string sTitle = collection["BookViewModel.Title"].ToString();
                string sAuthor = collection["BookViewModel.Author"].ToString();
                int iYearPublished = System.Convert.ToInt32(collection["BookViewModel.YearPublished"].ToString());
                string sGenre = collection["BookViewModel.Genre"].ToString();
                string sEdition = collection["BookViewModel.Edition"].ToString();
                string sISBN = collection["BookViewModel.ISBN"].ToString();
                string sLocation = collection["BookViewModel.Location"].ToString();

                BookViewModel book = await _db.Books.Where(a => a.ID == Id).SingleOrDefaultAsync();
                if (book != null)
                {
                    book.Title = sTitle;
                    book.Author = sAuthor;
                    book.Edition = sEdition;
                    book.Genre = sGenre;
                    book.ISBN = sISBN;
                    book.Location = sLocation;
                    book.YearPublished = iYearPublished;
                    book.DateUpdated = DateTime.Now;
                    _db.Update(book);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Details", "Book", new { id = book.ID });
                }

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Book/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}