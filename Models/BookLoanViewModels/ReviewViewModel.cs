using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookLoan.Models
{
    public class ReviewViewModel
    {
        public int ID { get; set; }

        public string Reviewer { get; set; }

        public string Comment { get; set; }

        public int Rating { get; set; }

        public DateTime DateReviewed { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public int BookID { get; set; }

        public BookViewModel Book { get; set; }

    }
}

