using DbProject.Entities.Authentication;
using Microsoft.EntityFrameworkCore;

namespace DbProject
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=Diplom;Persist Security Info=True;User ID=sa;Password=Password_01");
        }
    }
}
