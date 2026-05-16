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

        public DbSet<PracticeOffer> PracticeOffers { get; set; }

        public DbSet<JobApplicationStatus> ApplicationStatuses { get; set; }

        public DbSet<JobApplication> Applications { get; set; }

        public DbSet<Chat> Chats { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<FavoriteVacancy> FavoriteVacancies { get; set; }

        public DbSet<ResumeView> ResumeViews { get; set; }

        public DbSet<VacancyView> VacancyViews { get; set; }

        public DbSet<University> Universities { get; set; }

        public DbSet<EducationalProgram> EducationalPrograms { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<StudentGroup> StudentGroups { get; set; }

        public DbSet<StudentGroupApplication> StudentGroupApplications { get; set; }

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

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.HasKey(c => c.UserId);
                entity.HasOne(c => c.User)
                    .WithOne()
                    .HasForeignKey<Teacher>(c => c.UserId);
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

            modelBuilder.Entity<Resume>()
                .HasMany(r => r.Applications)
                .WithOne(a => a.Resume)
                .HasForeignKey(a => a.ResumeId);

            modelBuilder.Entity<Resume>()
                .HasMany(r => r.Views)
                .WithOne(v => v.Resume)
                .HasForeignKey(v => v.ResumeId);

            modelBuilder.Entity<Vacancy>()
                .HasOne(v => v.Company)
                .WithMany()
                .HasForeignKey(v => v.CompanyId);

            modelBuilder.Entity<Vacancy>()
                .HasMany(v => v.Applications)
                .WithOne(a => a.Vacancy)
                .HasForeignKey(v => v.VacancyId);

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

            modelBuilder.Entity<Vacancy>()
                .HasMany(vacancy => vacancy.Views)
                .WithOne(view => view.Vacancy)
                .HasForeignKey(view => view.VacancyId);

            modelBuilder.Entity<PracticeOffer>()
                .HasOne(po => po.Company)
                .WithMany()
                .HasForeignKey(po => po.CompanyId);

            modelBuilder.Entity<PracticeOffer>()
                .HasOne(po => po.Specialization)
                .WithMany()
                .HasForeignKey(po => po.SpecializationId);

            modelBuilder.Entity<Chat>()
                .HasOne(c => c.Company)
                .WithMany()
                .HasForeignKey(c => c.CompanyId);

            modelBuilder.Entity<Chat>()
                .HasOne(c => c.StudentProfile)
                .WithMany()
                .HasForeignKey(c => c.StudentId);

            modelBuilder.Entity<Chat>()
                .HasOne(c => c.Vacancy)
                .WithMany()
                .HasForeignKey(c => c.VacancyId);

            modelBuilder.Entity<Chat>()
                .HasMany(c => c.Messages)
                .WithOne()
                .HasForeignKey(msg => msg.ChatId);

            modelBuilder.Entity<VacancyView>()
                .HasOne(r => r.StudentProfile)
                .WithMany()
                .HasForeignKey(r => r.StudentId);

            modelBuilder.Entity<StudentGroup>()
                .HasMany(g => g.StudentProfiles)
                .WithOne(sp => sp.Group)
                .HasForeignKey(sp => sp.GroupId);

            modelBuilder.Entity<StudentGroupApplication>()
                .HasOne(r => r.StudentProfile)
                .WithOne()
                .HasForeignKey<StudentGroupApplication>(r => r.StudentId);

            modelBuilder.Entity<StudentGroupApplication>()
                .HasOne(r => r.Group)
                .WithMany()
                .HasForeignKey(r => r.GroupId);
        }
    }
}
