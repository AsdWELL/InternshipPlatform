using InternshipPlatform.Application.Dtos.User;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Mappers
{
    public static class CompanyMapper
    {
        public static Company ToCompany(this RegisterCompanyRequest request)
        {
            return new Company
            {
                Inn = request.Inn,
                Name = request.Name
            };
        }
    }
}
