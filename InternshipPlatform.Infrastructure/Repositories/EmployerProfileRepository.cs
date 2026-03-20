using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class EmployerProfileRepository(InternshipPlatformContext context) : IEmployerProfileRepository
    {
        public async Task AddEmployer(EmployerProfile employerProfile)
        {
            await context.EmployerProfiles.AddAsync(employerProfile);
        }
    }
}
