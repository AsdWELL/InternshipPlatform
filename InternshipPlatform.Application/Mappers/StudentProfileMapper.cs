using InternshipPlatform.Application.Dtos.StudentProfile;
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
                GraduationYear = studentProfile?.Group?.GraduationYear,
                MaxLink = studentProfile.MaxLink,
                Phone = studentProfile.Phone,
                Specialization = studentProfile?.Group?.Specialization,
                TgLink = studentProfile.TgLink,
                University = studentProfile?.Group?.University.Name,
                VkLink = studentProfile.VkLink,
                AvatarPath = studentProfile.AvatarPath
            };
        }

        public static StudentProfileItem ToItem(this StudentProfile studentProfile)
        {
            return new StudentProfileItem
            {
                Id = studentProfile.UserId,
                Name = studentProfile.Name,
                Surname = studentProfile.Surname,
                Patronymic = studentProfile.Patronymic,
                AvatarPath = studentProfile.AvatarPath
            };
        }
    }

}
