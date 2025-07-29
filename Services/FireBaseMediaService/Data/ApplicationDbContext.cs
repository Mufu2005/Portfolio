using FirebaseMediaService.Models;
using Microsoft.EntityFrameworkCore;

namespace FirebaseMediaService.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<MediaDbModel> MediaTable { get; set; }
    }
}
