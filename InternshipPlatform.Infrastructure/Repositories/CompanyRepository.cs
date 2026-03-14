using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class CompanyRepository(InternshipPlatformContext context) : ICompanyRepository
    {
        public async Task<int> CreateCompany(Company company)
        {
            context.Companies.Add(company);

            await context.SaveChangesAsync();

            return company.Id;
        }

        public async Task<Company?> GetCompanyById(int id)
        {
            return await context.Companies
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateCompany(Company company)
        {
            var dbCompany = await context.Companies.FindAsync(company.Id);

            if (company.Inn != null)
                dbCompany.Inn = company.Inn;

            if (company.Name != null)
                dbCompany.Name = company.Name;

            if (company.Link != null)
                dbCompany.Link = company.Link;

            if (company.Description != null)
                dbCompany.Description = company.Description;

            await context.SaveChangesAsync();
        }
    }
}
