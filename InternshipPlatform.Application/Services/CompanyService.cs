using InternshipPlatform.Application.Dtos.Company;
using InternshipPlatform.Application.Exceptions.Company;
using InternshipPlatform.Application.Exceptions.Image;
using InternshipPlatform.Application.Interfaces;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Mappers;
using InternshipPlatform.Application.Utils;
using InternshipPlatform.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace InternshipPlatform.Application.Services
{
    public class CompanyService(
        ICompanyRepository companyRepository,
        IUnitOfWork unitOfWork,
        IImageService imageService) : ICompanyService
    {
        private const int MaxLogoSizeMb = 10;
        
        private async Task<Company> GetCompanyByEmployerIdOrThrow(int employerId)
        {
            var company = await companyRepository.GetCompanyByEmployerId(employerId)
                ?? throw new CompanyNotFoundException();

            return company;
        }
        
        public async Task<CompanyResponse> GetEmployerCompany(int employerId)
        {
            var company = await GetCompanyByEmployerIdOrThrow(employerId);

            return company.ToResponse();
        }

        public async Task<CompanyResponse> GetCompanyById(int id)
        {
            var company = await companyRepository.GetCompanyById(id)
                ?? throw new CompanyNotFoundException();

            return company.ToResponse();
        }

        public async Task UpdateCompany(UpdateCompanyRequest request)
        {
            var company = await companyRepository.GetCompanyForUpdateByEmployerId(request.EmployerId)
                ?? throw new CompanyNotFoundException();

            if (!string.IsNullOrWhiteSpace(request.Name))
                company.Name = StringNormalizer.NormalizeRequired(request.Name);

            if (request.Link is not null)
                company.Link = StringNormalizer.NormalizeOptional(request.Link);

            if (request.Description is not null)
                company.Description = StringNormalizer.NormalizeOptional(request.Description);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateCompanyLogoByEmployerId(int employerId, IFormFile logoFile)
        {
            var company = await GetCompanyByEmployerIdOrThrow(employerId);

            int maxLogoSizeBytes = MaxLogoSizeMb * 1024 * 1024;

            if (logoFile.Length == 0)
                throw new EmptyImageException();

            if (logoFile.Length > maxLogoSizeBytes)
                throw new MaxImageSizeException(MaxLogoSizeMb);

            var oldLogoPath = company.LogoPath;

            await using var fstream = logoFile.OpenReadStream();

            var newLogoPath = await imageService.SaveCompanyLogo(fstream, Path.GetExtension(logoFile.FileName));
            await companyRepository.UpdateCompanyLogo(company.Id, newLogoPath);

            try
            {
                await unitOfWork.SaveChangesAsync();
            }
            catch
            {
                await imageService.DeleteIfExists(newLogoPath);
                throw;
            }

            await imageService.DeleteIfExists(oldLogoPath);
        }

        public async Task DeleteCompanyLogoByEmployerId(int employerId)
        {
            var company = await GetCompanyByEmployerIdOrThrow(employerId);

            await companyRepository.UpdateCompanyLogo(company.Id, null);

            await imageService.DeleteIfExists(company.LogoPath);

            await unitOfWork.SaveChangesAsync();
        }
    }
}
