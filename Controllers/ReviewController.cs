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
    public class ReviewController : Controller
    {
        ApplicationDbContext _db;
        ILoanService _loanService;
        IReviewService _reviewService;

        public ReviewController(ApplicationDbContext db, 
            ILoanService loanService,
            IReviewService reviewService)
        {
            _db = db;
            _loanService = loanService;
            _reviewService = reviewService;
        }

        [HttpGet("api/[controller]/List")]
        public async Task<ActionResult> List()
        {
            List<BookLoan.Models.ReportViewModels.LoanedBookReportViewModel> userLoans = 
                new List<Models.ReportViewModels.LoanedBookReportViewModel>();

            userLoans = await _loanService.GetBooksLoanedByCurrentUser();
            return View(userLoans);
        }


        // GET: Review/Review/5
        [HttpGet("api/[controller]/Review/{id}")]
        public async Task<ActionResult> Review(int id)
        {
            BookLoan.Models.BookLoanViewModels.LoanedBookViewModel userLoan =
                new Models.BookLoanViewModels.LoanedBookViewModel();

            BookLoan.Views.Review.ReviewModel reviewModel = new Views.Review.ReviewModel(_db);
            //reviewModel.ReviewViewModel = 
            ReviewViewModel reviewViewModel = await _loanService.GetLoanForReview(id);
            return View(reviewViewModel);
        }


        // POST: Review/SaveReview
        [HttpPost("api/[controller]/Review")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Review(ReviewViewModel model)
        {
            try
            {
                // TODO: Add update logic here
                await _reviewService.SaveReview(model);
                return RedirectToAction("List", "Review");
            }
            catch (Exception ex)
            {
                return View();
            }
        }


        // GET: SearchBook
        [HttpGet("api/[controller]/Index")]
        public ActionResult Index()
        {
            return View();
        }

        // GET: SearchBook/Details/5
        [HttpGet("api/[controller]/Details/{id}")]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SearchBook/Create
        [HttpGet("api/[controller]/Create")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: SearchBook/Create
        [HttpPost("api/[controller]/Create/{ID}")]
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
        [HttpGet("api/[controller]/Edit/{id}")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SearchBook/Edit/5
        [HttpPost("api/[controller]/Edit/{id}")]
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
        [HttpGet("api/[controller]/Delete/{id}")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SearchBook/Delete/5
        [HttpPost("api/[controller]/Delete/{id}")]
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