using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class CompanyRepository(InternshipPlatformContext context) : ICompanyRepository
    {
        public async Task AddCompany(Company company)
        {
            await context.Companies.AddAsync(company);
        }

        public async Task<Company?> GetCompanyById(int id)
        {
            return await context.Companies
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Company?> GetCompanyForUpdateByEmployerId(int employerId)
        {
            return await context.EmployerProfiles
                .Where(ep => ep.UserId == employerId)
                .Select(ep => ep.Company)
                .FirstOrDefaultAsync();
        }

        public async Task<Company?> GetCompanyByEmployerId(int employerId)
        {
            return await context.EmployerProfiles
                .AsNoTracking()
                .Where(ep => ep.UserId == employerId)
                .Select(ep => ep.Company)
                .FirstOrDefaultAsync();
        }

        public async Task<Company?> GetCompanyByInn(string inn)
        {
            return await context.Companies
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Inn == inn);
        }

        public async Task UpdateCompanyLogo(int companyId, string? logoPath)
        {
            var dbCompany = await context.Companies.FindAsync(companyId);

            if (dbCompany == null)
                return;

            dbCompany.LogoPath = logoPath;
        }

        public async Task DeleteCompany(int companyId)
        {
            var company = await context.Companies.FindAsync(companyId);

            if (company == null)
                return;

            context.Companies.Remove(company);
        }
    }
}
