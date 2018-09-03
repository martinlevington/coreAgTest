using Microsoft.EntityFrameworkCore;
using RPS.Presentation.Server.Models;

namespace RPS.Presentation.Server.Data
{
    public class SpaDbContext : DbContext
    {
        public SpaDbContext(DbContextOptions<SpaDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        //List of DB Models - Add your DB models here
        public DbSet<User> User { get; set; }
    }
}
