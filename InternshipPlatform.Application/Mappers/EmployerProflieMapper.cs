using InternshipPlatform.Application.Dtos.EmployerProflie;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Mappers
{
    public static class EmployerProflieMapper
    {
        public static EmployerProflieResponse ToResponse(this EmployerProfile employerProfile)
        {
            return new EmployerProflieResponse
            {
                UserId = employerProfile.UserId,
                Email = employerProfile.User.Email,
                Company = employerProfile.Company.ToResponse()
            };
        }
    }
}
