using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFTest.Readmodels
{
    public class BookDbContext: DbContext
    {
        public DbSet<Book> Books { get; set; }

        public BookDbContext(DbContextOptions options) : base(options) { }
    }

    public class Book
    {
        public string BookId { get; set; }
        public string Title { get; set; }
    }
}
