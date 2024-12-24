using LoginLogout.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoginLogout.DbContextConn
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }

        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<UserInformation> UserInformation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}