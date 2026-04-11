using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class EmployerProfileRepository(InternshipPlatformContext context) : IEmployerProfileRepository
    {
        public async Task<bool> IsEmployerProfileExists(int employerId)
        {
            return await context.EmployerProfiles
                .AnyAsync(ep => ep.UserId == employerId);
        }

        public async Task AddEmployerProfile(EmployerProfile employerProfile)
        {
            await context.EmployerProfiles.AddAsync(employerProfile);
        }

        public async Task<EmployerProfile?> GetEmployerProfileById(int employerId)
        {
            return await context.EmployerProfiles
                .AsNoTracking()
                .Include(ep => ep.Company)
                .Include(ep => ep.User)
                .FirstOrDefaultAsync(ep => ep.UserId == employerId);
        }

        public async Task<EmployerProfile?> GetEmployerProfileForUpdate(int employerId)
        {
            return await context.EmployerProfiles
                .Include(ep => ep.User)
                .FirstOrDefaultAsync(ep => ep.UserId == employerId);
        }
    }
}
