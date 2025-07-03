using Microsoft.EntityFrameworkCore;
using PhotographyService.Models;

namespace PhotographyService.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<PhotoModel> photoDb {  get; set; }
    }
}
