using Microsoft.EntityFrameworkCore;
using Parky.api.Models;

namespace Parky.api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<NationalPark> NationalParks { get; set; }
    }
}
