using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface IEmployerProfileRepository
    {
        Task<bool> IsEmployerProfileExists(int employerId);
        
        Task AddEmployerProfile(EmployerProfile employerProfile);

        Task<EmployerProfile?> GetEmployerProfileById(int employerId);

        Task UpdateEmployerProfile(EmployerProfile employerProfile);
    }
}
