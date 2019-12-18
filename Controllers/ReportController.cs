using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookLoan.Data;
using BookLoan.Services;

namespace BookLoan.Controllers
{
    public class ReportController : Controller
    {
        ApplicationDbContext _db;
        IReportService _reportService;

        public ReportController(ApplicationDbContext db, IReportService reportService)
        {
            _db = db;
            _reportService = reportService;
        }


        [HttpGet]
        public async Task<ActionResult> LoanReport()
        {
            return View(await _reportService.OnLoanReport());
        }


        [HttpGet]
        public async Task<ActionResult> MyLoanReport()
        {
            return View(await _reportService.MyOnLoanReport());
        }


        // GET: Report
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Report/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Report/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Report/Create
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

        // GET: Report/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Report/Edit/5
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

        // GET: Report/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Report/Delete/5
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