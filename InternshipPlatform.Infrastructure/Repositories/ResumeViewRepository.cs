using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class ResumeViewRepository(InternshipPlatformContext context) : IResumeViewRepository
    {
        public async Task AddResumeView(int companyId, int resumeId)
        {
            var now = DateTime.UtcNow;
            var threshold = now.AddMinutes(-5);

            var alreadyViewedRecently = await context.ResumeViews
                .AsNoTracking()
                .AnyAsync(v =>
                    v.CompanyId == companyId &&
                    v.ResumeId == resumeId &&
                    v.ViewDate >= threshold);

            if (alreadyViewedRecently)
                return;

            await context.ResumeViews.AddAsync(new ResumeView
            {
                CompanyId = companyId,
                ResumeId = resumeId,
                ViewDate = now
            });
        }

        public async Task<Dictionary<int, int>> GetResumesViewsCount(int studentId)
        {
            return await context.ResumeViews
                .AsNoTracking()
                .Where(v => v.Resume.StudentId == studentId)
                .GroupBy(v => v.ResumeId)
                .Select(g => new
                {
                    ResumeId = g.Key,
                    Count = g.Count()
                })
                .ToDictionaryAsync(r => r.ResumeId, r => r.Count);
        }

        public async Task<List<ResumeView>> GetResumeViews(int resumeId)
        {
            return await context.ResumeViews
                .AsNoTracking()
                .Where(v => v.ResumeId == resumeId)
                .Include(v => v.Company)
                .OrderByDescending(v => v.ViewDate)
                .ToListAsync();
        }
    }
}
