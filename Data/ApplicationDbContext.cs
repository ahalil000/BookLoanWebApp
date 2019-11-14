using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BookLoan.Models;

namespace BookLoan.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Core Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Core Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);


            builder.Entity<BookViewModel>().ToTable("Books");
            builder.Entity<ReviewViewModel>().ToTable("Reviews");
            builder.Entity<LoanViewModel>().ToTable("Loans");

            builder.Entity<ReviewViewModel>().
                HasOne(p => p.Book).
                WithMany(q => q.Reviews).
                HasForeignKey(p => p.BookID).HasConstraintName("FK_Book_Review");

            builder.Entity<LoanViewModel>().
                HasOne(p => p.Book).
                WithMany(q => q.Loans).    
                HasForeignKey(p => p.BookID).HasConstraintName("FK_Book_Loan");
        }

        public DbSet<BookViewModel> Books { get; set; }
        public DbSet<LoanViewModel> Loans { get; set; }
        public DbSet<ReviewViewModel> Reviews { get; set; }

    }
}
