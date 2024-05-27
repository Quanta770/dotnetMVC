using Microsoft.EntityFrameworkCore;
using WebApp_Razor.Models;


namespace WebApp_Razor.Data
{
    public class ApplicationDbContext : DbContext
    {
        // contructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //table name is method name
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(

                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Sci Fi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Historical", DisplayOrder = 3 }

                );
        }
    }
}
