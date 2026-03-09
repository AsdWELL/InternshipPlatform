using InternshipPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure
{
    public class InternshipPlatformContext(DbContextOptions<InternshipPlatformContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<StudentProfile> StudentProfiles { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<EmployerProfile> EmployerProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentProfile>()
                .HasKey(sp => sp.UserId);

            modelBuilder.Entity<EmployerProfile>()
                .HasKey(ep => ep.UserId);
        }
    }
}
