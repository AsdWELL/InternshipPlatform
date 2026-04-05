using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class SpecializationRepository(InternshipPlatformContext context) : ISpecializationRepository
    {
        public async Task<bool> IsSpecializationExists(int id)
        {
            return await context.Specializations
                .AsNoTracking()
                .AnyAsync(sp => sp.Id == id);
        }

        public async Task<List<Specialization>> GetAllSpecializations()
        {
            return await context.Specializations
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Specialization>> SearchSpecialization(string specializationName)
        {
            var search = specializationName.Trim().ToLowerInvariant();
            
            return await context.Specializations
                .AsNoTracking()
                .Where(sp => sp.Name.ToLower().Contains(search))
                .ToListAsync();
        }
    }
}
