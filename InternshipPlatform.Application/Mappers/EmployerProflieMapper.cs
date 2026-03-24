using InternshipPlatform.Application.Dtos.EmployerProflie;
using InternshipPlatform.Application.Utils;
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
                IsVerified = employerProfile.User.IsVerified,
                Company = employerProfile.Company.ToResponse()
            };
        }

        public static EmployerProfile ToDomain(
            this UpdateEmployerProflieRequest request,
            string? passwordHash)
        {
            return new EmployerProfile
            {
                UserId = request.UserId,
                User = new User
                {
                    Id = request.UserId,
                    Email = StringNormalizer.NormalizeToLower(request.Email),
                    PasswordHash = passwordHash
                }
            };
        }
    }
}
