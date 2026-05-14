using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class EducationalProgramsRepository(InternshipPlatformContext context) : IEducationalProgramsRepository
    {
        public Task<List<EducationalProgram>> GetAllUniversityEducationalPrograms(int teacherId)
        {
            return context.EducationalPrograms
                .AsNoTracking()
                .Where(ep => ep.UniversityId == context.Teachers
                    .AsNoTracking()
                    .Where(t => t.UserId == teacherId)
                    .Select(t => t.UniversityId)
                    .FirstOrDefault())
                .ToListAsync();
        }

        public Task<EducationalProgram?> GetEducationalProgramById(int id)
        {
            return context.EducationalPrograms
                .AsNoTracking()
                .FirstOrDefaultAsync(ep => ep.Id == id);
        }

        public async Task<List<EducationalProgram>> SearchEducationalPrograms(int teacherId, string educationalProgramName)
        {
            var search = educationalProgramName.Trim().ToLowerInvariant();

            return await context.EducationalPrograms
                .AsNoTracking()
                .Where(ep => ep.UniversityId == context.Teachers
                    .AsNoTracking()
                    .Where(t => t.UserId == teacherId)
                    .Select(t => t.UniversityId)
                    .FirstOrDefault() && ep.Name.ToLower().Contains(search))
                .ToListAsync();
        }
    }
}
