using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class EmployerProfileRepository(InternshipPlatformContext context) : IEmployerProfileRepository
    {
        public async Task<int> CreateEmployer(EmployerProfile employerProfile)
        {
            context.EmployerProfiles.Add(employerProfile);

            await context.SaveChangesAsync();

            return employerProfile.UserId;
        }
    }
}
