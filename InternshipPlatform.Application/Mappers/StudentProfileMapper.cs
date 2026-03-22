using InternshipPlatform.Application.Dtos.StudentProfile;
using InternshipPlatform.Application.Utils;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Mappers
{
    public static class StudentProfileMapper
    {
        public static StudentProfileResponse ToResponse(this StudentProfile studentProfile)
        {
            return new StudentProfileResponse
            {
                UserId = studentProfile.UserId,
                Email = studentProfile.User.Email,
                IsVerified = studentProfile.User.IsVerified,
                Name = studentProfile.Name,
                Surname = studentProfile.Surname,
                Patronymic = studentProfile.Patronymic,
                BirthdayDate = studentProfile.BirthdayDate,
                GithubLink = studentProfile.GithubLink,
                GraduationYear = studentProfile.GraduationYear,
                MaxLink = studentProfile.MaxLink,
                Phone = studentProfile.Phone,
                Specialization = studentProfile.Specialization,
                TgLink = studentProfile.TgLink,
                University = studentProfile.University,
                VkLink = studentProfile.VkLink,
                AvatarPath = studentProfile.AvatarPath
            };
        }

        public static StudentProfile ToDomain(
            this UpdateStudentProfileRequest request,
            string? passwordHash)
        {
            return new StudentProfile
            {
                UserId = request.UserId,
                User = new User
                {
                    Id = request.UserId,
                    Email = StringNormalizer.NormalizeToLower(request.Email),
                    PasswordHash = passwordHash
                },
                Name = StringNormalizer.NormalizeName(request.Name),
                Surname = StringNormalizer.NormalizeName(request.Surname),
                Patronymic = StringNormalizer.NormalizeName(request.Patronymic),
                BirthdayDate = request.BirthdayDate,
                GithubLink = StringNormalizer.NormalizeToLower(request.GithubLink),
                GraduationYear = request.GraduationYear,
                Phone = StringNormalizer.NormalizePhone(request.Phone),
                MaxLink = StringNormalizer.NormalizeToLower(request.MaxLink),
                TgLink = StringNormalizer.NormalizeToLower(request.TgLink),
                VkLink = StringNormalizer.NormalizeToLower(request.VkLink),
                Specialization = StringNormalizer.NormalizeOptional(request.Specialization),
                University = StringNormalizer.NormalizeOptional(request.University)!
            };
        }
    }
}
