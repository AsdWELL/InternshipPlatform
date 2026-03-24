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

        public async Task UpdateEmployerProfile(EmployerProfile employerProfile)
        {
            var employer = await context.EmployerProfiles
                .Include(ep => ep.User)
                .FirstOrDefaultAsync(ep => ep.UserId == employerProfile.UserId);

            if (employerProfile is null)
                return;

            if (employerProfile.User.Email is not null)
                employer.User.Email = employerProfile.User.Email;

            if (employerProfile.User.PasswordHash is not null)
                employer.User.PasswordHash = employerProfile.User.PasswordHash;
        }
    }
}
