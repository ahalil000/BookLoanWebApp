using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLoan.Data;
using BookLoan.Domain;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using BookLoan.Models;


namespace BookLoan.Services
{
    public class ReviewService: IReviewService
    {
        ApplicationDbContext _db;
        ILogger _logger;
        IBookService _bookService;

        public ReviewService(ApplicationDbContext db,
            IBookService bookService,
            ILogger<ReviewService> logger)
        {
            _db = db;
            _logger = logger;
            _bookService = bookService;

        }


        /// <summary>
        /// SaveReview()
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task SaveReview(ReviewViewModel vm)
        {
            ReviewViewModel reviewViewModel = new ReviewViewModel()
            {
                BookID = vm.BookID,
                Heading = vm.Heading,
                Comment = vm.Comment,
                Rating = vm.Rating,
                DateReviewed = vm.DateReviewed,
                DateCreated = DateTime.Now,
                Reviewer = vm.Reviewer
            };
            _db.Add(reviewViewModel);
            await _db.SaveChangesAsync();
        }


        /// <summary>
        /// UpdateReview()
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task UpdateReview(ReviewViewModel vm)
        {
            vm.DateUpdated = DateTime.Now;
            _db.Update(vm);
            await _db.SaveChangesAsync();
        }


        /// <summary>
        /// WasBookReviewedByUser()
        /// </summary>
        /// <param name="bookid"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> WasBookReviewedByUser(int bookid, string user)
        {
            return await _db.Reviews.Where(r => r.BookID == bookid && r.Reviewer == user).AnyAsync();
        }


        /// <summary>
        /// GetReviewerBookRating
        /// </summary>
        /// <param name="bookid"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<int> GetReviewerBookRating(int bookid, string user)
        {
            ReviewViewModel review = await _db.Reviews.Where(r => r.BookID == bookid && r.Reviewer == user).SingleOrDefaultAsync();
            if (review != null)
            {
                return review.Rating;
            }
            return 0;
        }


        /// <summary>
        /// GetReviewerStartBookRating()
        /// </summary>
        /// <param name="bookid"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<string> GetReviewerBookStarRating(int bookid, string user)
        {
            int rating = await GetReviewerBookRating(bookid, user);
            if (rating > 0)
                return new string('*', rating);
            return string.Empty;
        }

    }
}
