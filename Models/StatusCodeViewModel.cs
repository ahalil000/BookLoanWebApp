using System;

namespace BookLoan.Models
{
    public class StatusCodeViewModel : ErrorViewModel
    {
        public string OriginalURL { get; set; }

        public bool ShowOriginalURL { get; set; }

        public string ErrorStatusCode { get; set; }
    }
}