using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookLoan.Data;
using BookLoan.Models;
using BookLoan.Views.Loan;

namespace BookLoan.Controllers
{
    public class LoanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoanController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LoanViewModels
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Loans.Include(l => l.Book);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: LoanViewModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanViewModel = await _context.Loans
                .Include(l => l.Book)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (loanViewModel == null)
            {
                return NotFound();
            }

            BookLoan.Views.Loan.DetailsModel detailsModel = new DetailsModel(_context);
            detailsModel.LoanViewModel = loanViewModel;

            return View(detailsModel);
        }


        // GET: LoanViewModels/Create
        public async Task<IActionResult> Create(int id)
        {
            //ViewData["BookID"] = new SelectList(_context.Books, "ID", "Author");
            LoanViewModel lvm = new LoanViewModel();
            lvm.DateLoaned = DateTime.Now;
            lvm.DateReturn = DateTime.Now;
            lvm.DateCreated = DateTime.Now;
            lvm.DateDue = DateTime.Now;
            lvm.OnShelf = true;
            lvm.LoanedBy = User.Identity.Name;
            lvm.BookID = id;
            BookLoan.Views.Loan.CreateModel createModel = new CreateModel(_context);
            createModel.LoanViewModel = lvm;
            return View(createModel);
        }

        // GET: LoanViewModels/Return
        public async Task<IActionResult> Return(int id)
        {
            LoanViewModel lvm = new LoanViewModel();
            BookViewModel bvm = new BookViewModel();

            var book = await _context.Books
                .SingleOrDefaultAsync(m => m.ID == id);

            if (book == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans
                .Where(m => m.BookID == book.ID).ToListAsync();

            if (loan == null)
            {
                return NotFound();
            }

            foreach (LoanViewModel item in loan.OrderByDescending(o => o.DateReturn))
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

            BookLoan.Views.Loan.ReturnModel returnModel = new ReturnModel(_context);
            returnModel.LoanViewModel = lvm;
            returnModel.BookViewModel = bvm;

            return View(returnModel);
        }



        // POST: LoanViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,LoanedBy,DateLoaned,DateReturn,OnShelf,DateCreated,DateDue,DateUpdated,BookID")] LoanViewModel loanViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loanViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Loan", new { id = loanViewModel.ID });
            }
            ViewData["BookID"] = new SelectList(_context.Books, "ID", "Author", loanViewModel.BookID);
            return View(loanViewModel);
        }


        // POST: LoanViewModels/Return
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return([Bind("ID,LoanedBy,DateLoaned,DateReturn,DateDue,OnShelf,DateCreated,DateUpdated,BookID")] LoanViewModel loanViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loanViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanViewModelExists(loanViewModel.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Loan", new { id = loanViewModel.ID });
            }
            ViewData["BookID"] = new SelectList(_context.Books, "ID", "Author", loanViewModel.BookID);
            return View(loanViewModel);

        }


        // GET: LoanViewModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanViewModel = await _context.Loans.SingleOrDefaultAsync(m => m.ID == id);
            if (loanViewModel == null)
            {
                return NotFound();
            }
            ViewData["BookID"] = new SelectList(_context.Books, "ID", "Author", loanViewModel.BookID);
            return View(loanViewModel);
        }

        // POST: LoanViewModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LoanedBy,DateLoaned,DateReturn,OnShelf,DateCreated,DateUpdated,BookID")] LoanViewModel loanViewModel)
        {
            if (id != loanViewModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loanViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanViewModelExists(loanViewModel.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Book", new { id = loanViewModel.BookID });
            }
            ViewData["BookID"] = new SelectList(_context.Books, "ID", "Author", loanViewModel.BookID);
            return View(loanViewModel);
        }

        // GET: LoanViewModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanViewModel = await _context.Loans
                .Include(l => l.Book)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (loanViewModel == null)
            {
                return NotFound();
            }

            return View(loanViewModel);
        }

        // POST: LoanViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loanViewModel = await _context.Loans.SingleOrDefaultAsync(m => m.ID == id);
            _context.Loans.Remove(loanViewModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoanViewModelExists(int id)
        {
            return _context.Loans.Any(e => e.ID == id);
        }
    }
}
