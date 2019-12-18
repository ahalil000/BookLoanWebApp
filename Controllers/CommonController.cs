using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookLoan.Models;
using Microsoft.AspNetCore.Authorization;
using BookLoan.Views.Common;
using Microsoft.AspNetCore.Http.Headers;

namespace BookLoan.Controllers
{
    public class CommonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]   
        public IActionResult Error(string errorMessage)
        {
            BookLoan.Views.Common.ErrorModel errorModel = new BookLoan.Views.Common.ErrorModel() 
            {
                ExceptionMessage = errorMessage,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            ViewData["Title"] = "Error";
            return View(errorModel);
        }

        [HttpGet]
        public IActionResult StatusCode(int? code = null)
        {
            string referer = Request.Headers["Referer"].ToString();
            BookLoan.Models.StatusCodeViewModel statuscodeModel = new BookLoan.Models.StatusCodeViewModel()
            {
                OriginalURL = referer,
                ErrorStatusCode = "",
                ExceptionMessage = "An error has occurred",
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            if (code.HasValue)
            {
                statuscodeModel.ErrorStatusCode = code.ToString();
                ViewData["Title"] = "Error " + code.ToString();
                return View(statuscodeModel);
            }
            statuscodeModel.ExceptionMessage = "";
            return View(statuscodeModel);
        }

    }
}
