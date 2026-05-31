using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface IEmployerProfileRepository
    {
        Task<bool> IsEmployerProfileExists(int employerId);
        
        Task AddEmployerProfile(EmployerProfile employerProfile);

        Task<EmployerProfile?> GetEmployerProfileById(int employerId);

        Task<EmployerProfile?> GetEmployerProfileForUpdate(int employerId);

        Task<string?> GetEmployerEmailByCompanyId(int companyId);
    }
}
