using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface ICompanyRepository
    {
        Task AddCompany(Company company);

        Task<Company?> GetCompanyById(int id);

        Task<Company?> GetCompanyForUpdateByEmployerId(int employerId);

        Task<Company?> GetCompanyByEmployerId(int employerId);

        Task<Company?> GetCompanyByInn(string inn);

        Task UpdateCompanyLogo(int companyId, string? logoPath);

        Task DeleteCompany(int companyId);
    }
}
