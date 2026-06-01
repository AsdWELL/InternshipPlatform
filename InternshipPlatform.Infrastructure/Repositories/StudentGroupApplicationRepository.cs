using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class StudentGroupApplicationRepository(InternshipPlatformContext context) : IStudentGroupApplicationRepository
    {
        public Task<bool> IsStudentHasGroupApplication(int studentId)
        {
            return context.StudentGroupApplications
                .AsNoTracking()
                .AnyAsync(ga => ga.StudentId == studentId);
        }

        public Task<bool> IsStudentOwnsGroupApplication(int studentId, int applicationId)
        {
            return context.StudentGroupApplications
                .AsNoTracking()
                .AnyAsync(ga => ga.Id == applicationId && ga.StudentId == studentId);
        }

        public async Task AddStudentGroupApplication(StudentGroupApplication application)
        {
            await context.StudentGroupApplications.AddAsync(application);
        }

        public async Task DeleteStudentGroupApplicationById(int applicationId)
        {
            var application = await context.StudentGroupApplications.FindAsync(applicationId);

            if (application is null)
                return;

            context.StudentGroupApplications.Remove(application);
        }

        public Task DeleteStudentGroupApplication(StudentGroupApplication studentGroupApplication)
        {
            context.StudentGroupApplications.Remove(studentGroupApplication);

            return Task.CompletedTask;
        }

        public Task<List<StudentGroupApplication>> GetCuratorGroupApplications(int curatorId)
        {
            return context.StudentGroupApplications
                .AsNoTracking()
                .Where(ga => ga.Group.CuratorId == curatorId)
                .Include(ga => ga.StudentProfile)
                    .ThenInclude(sp => sp.User)
                .Include(ga => ga.Group)
                .OrderByDescending(ga => ga.CreatedAt)
                .ToListAsync();
        }

        public Task<StudentGroupApplication?> GetStudentGroupApplication(int studentId)
        {
            return context.StudentGroupApplications
                .AsNoTracking()
                .Where(ga => ga.StudentId == studentId)
                .Include(ga => ga.Group)
                    .ThenInclude(g => g.University)
                .Include(ga => ga.Group)
                    .ThenInclude(g => g.EducationalProgram)
                .FirstOrDefaultAsync();
        }

        public Task<StudentGroupApplication?> GetCuratorGroupApplicationForUpdate(int curatorId, int applicationId)
        {
            return context.StudentGroupApplications
                .Include(a => a.Group)
                .Include(a => a.StudentProfile)
                    .ThenInclude(sp => sp.User)
                .FirstOrDefaultAsync(a =>
                    a.Id == applicationId &&
                    a.Group.CuratorId == curatorId);
        }
    }
}
