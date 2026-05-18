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

        public DbSet<PracticeMaterial> PracticeMaterials { get; set; }

        public DbSet<JobApplicationStatus> ApplicationStatuses { get; set; }

        public DbSet<JobApplication> JobApplications { get; set; }

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

        public DbSet<PracticePeriod> PracticePeriods { get; set; }

        public DbSet<PracticeApplication> PracticeApplicaitons { get; set; }

        public DbSet<StudentPractice> StudentPractices { get; set; }

        public DbSet<PracticeSubmissionStatus> PracticeSubmissionStatuses { get; set; }

        public DbSet<PracticeSubmission> PracticeSubmissions { get; set; }

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

            modelBuilder.Entity<PracticeOffer>()
                .HasMany(po => po.Materials)
                .WithOne()
                .HasForeignKey(pm => pm.PracticeOfferId);

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

            modelBuilder.Entity<PracticePeriod>()
                .HasOne(p => p.Supervisor)
                .WithMany()
                .HasForeignKey(p => p.SupervisorId);

            modelBuilder.Entity<PracticePeriod>()
                .HasOne(p => p.EducationalProgram)
                .WithMany()
                .HasForeignKey(p => p.EducationalProgramId);

            modelBuilder.Entity<StudentGroup>()
                .HasMany(g => g.PracticePeriods)
                .WithMany(p => p.StudentGroups)
                .UsingEntity(
                    "PracticePeriodsGroup",
                    r => r.HasOne(typeof(PracticePeriod))
                          .WithMany()
                          .HasForeignKey("PracticePeriodId"),
                    l => l.HasOne(typeof(StudentGroup))
                          .WithMany()
                          .HasForeignKey("StudentGroupId"),
                    j => j.HasKey("PracticePeriodId", "StudentGroupId"));

            modelBuilder.Entity<PracticeApplication>()
                .HasOne(pa => pa.Student)
                .WithMany()
                .HasForeignKey(pa => pa.StudentId);
            
            modelBuilder.Entity<PracticeApplication>()
                .HasOne(pa => pa.PracticeOffer)
                .WithMany()
                .HasForeignKey(pa => pa.PracticeOfferId);

            modelBuilder.Entity<PracticeApplication>()
                .HasOne(pa => pa.PracticePeriod)
                .WithMany()
                .HasForeignKey(pa => pa.PracticePeriodId);

            modelBuilder.Entity<StudentPractice>()
                .HasOne(sp => sp.Student)
                .WithMany()
                .HasForeignKey(sp => sp.StudentId);

            modelBuilder.Entity<StudentPractice>()
                .HasOne(sp => sp.PracticeOffer)
                .WithMany()
                .HasForeignKey(sp => sp.PracticeOfferId);

            modelBuilder.Entity<StudentPractice>()
                .HasOne(sp => sp.PracticePeriod)
                .WithMany()
                .HasForeignKey(sp => sp.PracticePeriodId);

            modelBuilder.Entity<PracticeSubmission>()
                .HasOne(ps => ps.StudentPractice)
                .WithOne(sp => sp.PracticeSubmission)
                .HasForeignKey<PracticeSubmission>(ps => ps.StudentPracticeId);

            modelBuilder.Entity<PracticeSubmission>()
                .HasOne(ps => ps.Status)
                .WithMany()
                .HasForeignKey(ps => ps.StatusId);
        }
    }
}
