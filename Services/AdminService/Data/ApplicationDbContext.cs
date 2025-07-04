using AdminService.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }

        public DbSet<LoginModel> LoginTable { get; set; } 
    }
}
