using InternshipPlatform.Application.Dtos.User;
using InternshipPlatform.Application.Utils;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Mappers
{
    public static class UserMapper
    {
        public static User ToUser(
            this RegisterStudentRequest request,
            string passwordHash,
            Role role,
            string refreshToken,
            DateTime refreshTokenExpiredAt)
        {
            return new User
            {
                Email = StringNormalizer.NormalizeToLower(request.Email)!,
                PasswordHash = passwordHash,
                Role = role,
                RefreshToken = refreshToken,
                RefreshTokenExpiredAt = refreshTokenExpiredAt
            };
        }

        public static User ToUser(
            this RegisterCompanyRequest request,
            string passwordHash,
            Role role,
            string refreshToken,
            DateTime refreshTokenExpiredAt)
        {
            return new User
            {
                Email = StringNormalizer.NormalizeToLower(request.Email)!,
                PasswordHash = passwordHash,
                Role = role,
                RefreshToken = refreshToken,
                RefreshTokenExpiredAt = refreshTokenExpiredAt
            };
        }

        public static User ToUser(
            this RegisterCuratorRequest request,
            string passwordHash,
            Role role,
            string refreshToken,
            DateTime refreshTokenExpiredAt)
        {
            return new User
            {
                Email = StringNormalizer.NormalizeToLower(request.Email)!,
                PasswordHash = passwordHash,
                Role = role,
                RefreshToken = refreshToken,
                RefreshTokenExpiredAt = refreshTokenExpiredAt
            };
        }

        public static StudentProfile ToStudentProfile(
            this RegisterStudentRequest request,
            User user)
        {
            return new StudentProfile
            {
                User = user,
                Name = StringNormalizer.NormalizeName(request.Name)!,
                Surname = StringNormalizer.NormalizeName(request.Surname)!
            };
        }

        public static Curator ToCurator(
            this RegisterCuratorRequest request,
            User user)
        {
            return new Curator
            {
                User = user,
                Name = StringNormalizer.NormalizeName(request.Name)!,
                Surname = StringNormalizer.NormalizeName(request.Surname)!,
                UniversityId = request.UniversityId
            };
        }
    }
}
