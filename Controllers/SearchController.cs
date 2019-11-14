using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookLoan.Models;
using BookLoan.Data;

namespace BookLoan.Controllers
{
    public class SearchController : Controller
    {
        ApplicationDbContext _db;


        public SearchController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult Search(string BookGenre, string SearchString)
        {
            SearchBookViewModel sm = new SearchBookViewModel();

            if (String.IsNullOrEmpty(SearchString))
            {
                //sm.Book = new List<BookViewModel>();
                sm.Book = _db.Books.ToList();
                return View("SearchResult", sm);
            }
            if (!String.IsNullOrEmpty(SearchString))
                sm.Book = _db.Books.Where(a => a.Title.Contains(SearchString)).ToList();

            return View("SearchResult", sm);
            //return RedirectToAction("/SearchBook/Search");
        }

        [HttpPost]
        public ActionResult Search(SearchBookViewModel svm)
        {
            SearchBookViewModel sm = new SearchBookViewModel();

            sm.Book = _db.Books.Where(a => a.Title.Contains(svm.SearchString)).ToList();
            return View(sm);
            //return RedirectToAction("/SearchBook/Search");
        }


        //[HttpPost]
        //public ActionResult Search(string SearchString)
        //{
        //    SearchBookViewModel sm = new SearchBookViewModel();

        //    sm.Book = _db.Books.Where(a => a.Title.Contains(SearchString)).ToList();
        //    return View(sm);
        //    //return RedirectToAction("/SearchBook/Search");
        //}



        // GET: SearchBook
        public ActionResult Index()
        {
            return View();
        }

        // GET: SearchBook/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SearchBook/Create
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