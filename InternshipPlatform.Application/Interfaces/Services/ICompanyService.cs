using InternshipPlatform.Application.Dtos.Company;
using Microsoft.AspNetCore.Http;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface ICompanyService
    {
        Task<CompanyResponse> GetCompanyById(int id);

        Task UpdateCompany(UpdateCompanyRequest request);

        Task UpdateCompanyLogoByEmployerId(int employerId, IFormFile logoFile);

        Task DeleteCompanyLogoByEmployerId(int employerId);
    }
}
