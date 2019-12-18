using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookLoan.Models;
using BookLoan.Data;
using BookLoan.Services;

namespace BookLoan.Controllers
{
    public class SearchController : Controller
    {
        ApplicationDbContext _db;
        IBookService _bookService;

        public SearchController(ApplicationDbContext db, IBookService bookService)
        {
            _db = db;
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult> Search(string BookGenre, string SearchString)
        {
            try
            {

                SearchBookViewModel sm = new SearchBookViewModel();

                if (String.IsNullOrEmpty(SearchString))
                {
                    //sm.Book = new List<BookViewModel>();
                    sm.Book = await _bookService.GetBooks(); // _db.Books.ToList();
                    return View("SearchResult", sm);
                }
                if (!String.IsNullOrEmpty(SearchString))
                    sm.Book = await _bookService.GetBooksFilter(SearchString); // _db.Books.Where(a => a.Title.Contains(SearchString)).ToList();

                return View("SearchResult", sm);
            }
            catch (Exception ex)
            {
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    return RedirectToAction("Error", "Common", new { errorMessage = ex.Message.ToString() } );
                }
                return RedirectToAction("Error", "Common", new { errorMessage = "Error running searching. Please contact support." });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Search(SearchBookViewModel svm)
        {
            SearchBookViewModel sm = new SearchBookViewModel();

            try
            {
                sm.Book = await _bookService.GetBooksFilter(svm.SearchString); // _db.Books.Where(a => a.Title.Contains(svm.SearchString)).ToList();
                return View(sm);
            }
            catch
            {
                return RedirectToPage("Error");
            }
        }
        //return RedirectToAction("/SearchBook/Search");


        //[HttpPost]
        //public ActionResult Search(string SearchString)
        //{
        //    SearchBookViewModel sm = new SearchBookViewModel();

        //    sm.Book = _db.Books.Where(a => a.Title.Contains(SearchString)).ToList();
        //    return View(sm);
        //    //return RedirectToAction("/SearchBook/Search");
        //}



        // GET: SearchBook
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        // GET: SearchBook/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SearchBook/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: SearchBook/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SearchBook/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SearchBook/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SearchBook/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SearchBook/Delete/5
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