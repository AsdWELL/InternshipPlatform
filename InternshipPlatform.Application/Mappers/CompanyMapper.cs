using InternshipPlatform.Application.Dtos.User;
using InternshipPlatform.Application.Utils;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Mappers
{
    public static class CompanyMapper
    {
        public static Company ToCompany(this RegisterCompanyRequest request)
        {
            return new Company
            {
                Inn = StringNormalizer.NormalizeRequired(request.Inn),
                Name = StringNormalizer.NormalizeRequired(request.Name)
            };
        }
    }
}
