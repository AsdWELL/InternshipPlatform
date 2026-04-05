using InternshipPlatform.Application.Dtos.Company;
using InternshipPlatform.Application.Dtos.User;
using InternshipPlatform.Application.Utils;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Mappers
{
    public static class CompanyMapper
    {
        public static Company ToDomain(this RegisterCompanyRequest request)
        {
            return new Company
            {
                Inn = StringNormalizer.NormalizeRequired(request.Inn),
                Name = StringNormalizer.NormalizeRequired(request.CompanyName)
            };
        }

        public static CompanyResponse ToResponse(this Company company)
        {
            return new CompanyResponse
            {
                Id = company.Id,
                Inn = company.Inn,
                Name = company.Name,
                Description = company.Description,
                Link = company.Link,
                LogoUrl = company.LogoPath
            };
        }

        public static Company ToDomain(
            this UpdateCompanyRequest request,
            int companyId)
        {
            return new Company
            {
                Id = companyId,
                Inn = StringNormalizer.NormalizeOptional(request.Inn),
                Name = StringNormalizer.NormalizeOptional(request.Name),
                Description = StringNormalizer.NormalizeOptional(request.Description),
                Link = StringNormalizer.NormalizeToLower(request.Link)
            };
        }
    }
}
