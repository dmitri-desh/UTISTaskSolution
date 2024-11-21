using EmployeeServer.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeServer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Worker> Workers { get; set; }
    }
}
