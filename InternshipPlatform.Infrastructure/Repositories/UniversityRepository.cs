using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class UniversityRepository(InternshipPlatformContext context) : IUniversityRepository
    {
        public Task<bool> IsUniversityExists(int universityId)
        {
            return context.Universities
                .AsNoTracking()
                .AnyAsync(u => u.Id == universityId);
        }

        public Task<List<University>> GetAllUniversities()
        {
            return context.Universities
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<List<University>> SearchUniversities(string universityName)
        {
            var search = universityName.Trim().ToLowerInvariant();

            return context.Universities
                .AsNoTracking()
                .Where(u => u.Name.ToLower().Contains(search))
                .ToListAsync();
        }
    }
}
