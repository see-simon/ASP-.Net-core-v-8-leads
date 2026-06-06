using Microsoft.EntityFrameworkCore;
using SmartFibreAPI.Models;

namespace SmartFibreAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Lead> Leads { get; set; }
    }
}