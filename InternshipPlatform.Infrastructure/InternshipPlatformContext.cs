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

        public DbSet<Specialization> Specializations { get; set; }

        public DbSet<Skill> Skills { get; set; }

        public DbSet<WorkExperience> WorkExperiences { get; set; }

        public DbSet<Resume> Resumes { get; set; }

        public DbSet<Vacancy> Vacancies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentProfile>(entity =>
            {
                entity.HasKey(sp => sp.UserId);
                entity.HasOne(sp => sp.User)
                    .WithOne()
                    .HasForeignKey<StudentProfile>(sp => sp.UserId);
            });

            modelBuilder.Entity<EmployerProfile>(entity =>
            {
                entity.HasKey(ep => ep.UserId);
                entity.HasOne(ep => ep.User)
                    .WithOne()
                    .HasForeignKey<EmployerProfile>(ep => ep.UserId);
            });

            modelBuilder.Entity<Resume>()
                .HasOne(r => r.StudentProfile)
                .WithMany()
                .HasForeignKey(r => r.StudentId);

            modelBuilder.Entity<Resume>()
                .HasMany(r => r.Skills)
                .WithMany()
                .UsingEntity(
                    "SkillsToResume",
                    r => r.HasOne(typeof(Skill))
                          .WithMany()
                          .HasForeignKey("SkillId"),
                    l => l.HasOne(typeof(Resume))
                          .WithMany()
                          .HasForeignKey("ResumeId"),
                    j => j.HasKey("SkillId", "ResumeId"));

            modelBuilder.Entity<Resume>()
                .HasMany(r => r.WorkExperiences)
                .WithOne()
                .HasForeignKey(we => we.ResumeId);

            modelBuilder.Entity<Vacancy>()
                .HasOne(v => v.Company)
                .WithMany()
                .HasForeignKey(v => v.CompanyId);

            modelBuilder.Entity<Vacancy>()
                .HasMany(v => v.Skills)
                .WithMany()
                .UsingEntity(
                    "SkillsToVacancy",
                    r => r.HasOne(typeof(Skill))
                          .WithMany()
                          .HasForeignKey("SkillId"),
                    l => l.HasOne(typeof(Vacancy))
                          .WithMany()
                          .HasForeignKey("VacancyId"),
                    j => j.HasKey("SkillId", "VacancyId"));
        }
    }
}
