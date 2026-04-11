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
    }
}
