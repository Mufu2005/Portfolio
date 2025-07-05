using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VideographyService.Models;

namespace VideographyService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<VideoModel> videoTable { get; set; }
    }
}
