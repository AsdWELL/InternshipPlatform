using InternshipPlatform.Application.Dtos.User;
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
                Email = request.Email,
                PasswordHash = passwordHash,
                Role = role,
                RefreshToken = refreshToken,
                RefreshTokenExpiredAt = refreshTokenExpiredAt
            };
        }

        public static User ToUser(
            this RegisterEmployerRequest request,
            string passwordHash,
            Role role,
            string refreshToken,
            DateTime refreshTokenExpiredAt)
        {
            return new User
            {
                Email = request.Email,
                PasswordHash = passwordHash,
                Role = role,
                RefreshToken = refreshToken,
                RefreshTokenExpiredAt = refreshTokenExpiredAt
            };
        }

        public static StudentProfile ToStudentProfile(
            this RegisterStudentRequest request,
            int userId)
        {
            return new StudentProfile
            {
                UserId = userId,
                Name = request.Name,
                Surname = request.Surname
            };
        }
    }
}
