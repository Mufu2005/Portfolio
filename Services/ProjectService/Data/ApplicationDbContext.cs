using ProjectService.Models;
using Microsoft.EntityFrameworkCore;

namespace ProjectService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<ProjectModel> ProjectModels { get; set; }
    }


}
