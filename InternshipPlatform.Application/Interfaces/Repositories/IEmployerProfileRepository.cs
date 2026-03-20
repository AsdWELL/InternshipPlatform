using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface IEmployerProfileRepository
    {
        Task AddEmployer(EmployerProfile employerProfile);
    }
}
