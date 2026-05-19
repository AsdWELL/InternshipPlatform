using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class PracticePeriodRepository(InternshipPlatformContext context) : IPracticePeriodRepository
    {
        public async Task<PracticePeriod?> GetCurrentStudentPracticePeriod(int studentId)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var semesterStart = today.Month switch
            {
                >= 9 and <= 12 => new DateOnly(today.Year, 9, 1),
                >= 1 and <= 7 => new DateOnly(today.Year, 1, 1),
                _ => (DateOnly?)null
            };

            var semesterEnd = today.Month switch
            {
                >= 9 and <= 12 => new DateOnly(today.Year, 12, 31),
                >= 1 and <= 7 => new DateOnly(today.Year, 7, 31),
                _ => (DateOnly?)null
            };

            if (semesterStart is null || semesterEnd is null)
                return null;

            return await context.PracticePeriods
                .AsNoTracking()
                .Where(p =>
                    p.StartDate >= semesterStart.Value &&
                    p.StartDate <= semesterEnd.Value)
                .Where(p => p.StudentGroups.Any(g =>
                    g.StudentProfiles.Any(sp => sp.UserId == studentId)))
                .Include(p => p.EducationalProgram)
                .Include(p => p.Supervisor)
                .FirstOrDefaultAsync();
        }

        public async Task<List<PracticePeriod>> GetStudentPracticePeriods(int studentId)
        {
            return await context.PracticePeriods
                .AsNoTracking()
                .Where(p => p.StudentGroups.Any(g => g.StudentProfiles.Any(sp => sp.UserId == studentId)))
                .Include(p => p.EducationalProgram)
                .OrderByDescending(p => p.AcademicYearStart)
                .ThenByDescending(p => p.StartDate)
                .ToListAsync();
        }

        public async Task<PracticePeriod?> GetStudentPracticePeriodDetails(int studentId, int practicePeriodId)
        {
            return await context.PracticePeriods
                .AsNoTracking()
                .Where(p => p.Id == practicePeriodId)
                .Where(p => p.StudentGroups.Any(g => g.StudentProfiles.Any(sp => sp.UserId == studentId)))
                .Include(p => p.EducationalProgram)
                .Include(p => p.Supervisor)
                    .ThenInclude(s => s.User)
                .FirstOrDefaultAsync();
        }

        public async Task<List<PracticePeriod>> GetTeacherPracticePeriodsByAcademicYear(int teacherId, int academicYearStart)
        {
            return await context.PracticePeriods
                .AsNoTracking()
                .Where(p => p.SupervisorId == teacherId && p.AcademicYearStart == academicYearStart)
                .Include(p => p.StudentGroups)
                    .ThenInclude(g => g.StudentProfiles)
                .OrderBy(p => p.StartDate)
                .ToListAsync();
        }

        public async Task<PracticePeriod?> GetTeacherPracticePeriodWithGroup(int teacherId, int practicePeriodId, int groupId)
        {
            return await context.PracticePeriods
                .AsNoTracking()
                .Where(p => p.Id == practicePeriodId && p.SupervisorId == teacherId)
                .Where(p => p.StudentGroups.Any(g => g.Id == groupId))
                .Include(p => p.EducationalProgram)
                .Include(p => p.StudentGroups.Where(g => g.Id == groupId))
                    .ThenInclude(g => g.StudentProfiles)
                        .ThenInclude(sp => sp.User)
                .FirstOrDefaultAsync();
        }
    }
}
