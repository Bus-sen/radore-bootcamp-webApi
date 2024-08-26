using Microsoft.EntityFrameworkCore;

namespace JwtOrnek.Entities
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=jwtSampleRadore.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = 1,
                FirstName = "testUser",
                UserName = "testUser",
                Password = "testPassword"
            });
        }
    }
}
