using Microsoft.EntityFrameworkCore;

namespace Demoproject.Models
{
    public class AppDbContaxt :DbContext
    {
        private readonly DbContextOptions<AppDbContaxt> options;

        public AppDbContaxt(DbContextOptions<AppDbContaxt> options):base (options)
        {
            this.options = options;
        }
        public DbSet<UserDetails> User { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Activity>Activities { get; set; }
        public DbSet<Login> login { get; set; }
        //protected override  void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<Admin>().HasData(
        //        new Admin() { Email="admin", Password="admin" });
        //}
    }
}
