using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface ICompanyRepository
    {
        Task AddCompany(Company company);

        Task<Company?> GetCompanyById(int id);

        Task<Company?> GetCompanyByEmployerId(int employerId);

        Task UpdateCompany(Company company);

        Task UpdateCompanyLogo(int companyId, string logoPath);
    }
}
