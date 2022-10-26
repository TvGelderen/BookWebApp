using Microsoft.EntityFrameworkCore;
using BookWebApp.Models;

namespace BookWebApp.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public Microsoft.EntityFrameworkCore.DbSet<Book> Book_db { get; set; }
    }
}
