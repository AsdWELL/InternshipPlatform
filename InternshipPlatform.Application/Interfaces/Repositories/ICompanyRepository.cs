using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface ICompanyRepository
    {
        Task<int> CreateCompany(Company company);

        Task<Company> GetCompanyById(int id);
    }
}
