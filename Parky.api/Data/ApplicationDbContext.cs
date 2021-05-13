using Microsoft.EntityFrameworkCore;
using Parky.api.Models;
using Parky.api.Models.DTOs;

namespace Parky.api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<NationalParkDTO> NationalParks { get; set; }
    }
}
