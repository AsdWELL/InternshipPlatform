using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class VacancyViewRepository(InternshipPlatformContext context) : IVacancyViewRepository
    {
        public async Task AddVacancyView(int studentId, int vacancyId)
        {
            var now = DateTime.UtcNow;
            var threshold = now.AddMinutes(-5);

            var alreadyViewedRecently = await context.VacancyViews
                .AsNoTracking()
                .AnyAsync(v =>
                    v.StudentId == studentId&&
                    v.VacancyId == vacancyId &&
                    v.ViewDate >= threshold);

            if (alreadyViewedRecently)
                return;

            await context.VacancyViews.AddAsync(new VacancyView
            {
                StudentId = studentId,
                VacancyId = vacancyId,
                ViewDate = now
            });
        }

        public async Task<List<VacancyView>> GetStudentVacancyViewsHistory(int studentId)
        {
            return await context.VacancyViews
                .AsNoTracking()
                .Where(v => v.StudentId == studentId)
                .Include(v => v.Vacancy)
                    .ThenInclude(v => v.Company)
                .OrderByDescending(v => v.ViewDate)
                .ToListAsync();
        }

        public async Task<List<VacancyView>> GetVacancyViews(int vacancyId)
        {
            return await context.VacancyViews
                .AsNoTracking()
                .Where(v => v.VacancyId == vacancyId)
                .Include(v => v.StudentProfile)
                .OrderByDescending(v => v.ViewDate)
                .ToListAsync();
        }
    }
}
