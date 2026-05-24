using InternshipPlatform.Application.Dtos.Teacher;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Mappers
{
    public static class TeacherMapper
    {
        public static TeacherResponse ToResponse(this Teacher curator)
        {
            return new TeacherResponse
            {
                UserId = curator.UserId,
                Email = curator.User.Email,
                Name = curator.Name,
                Surname = curator.Surname,
                Patronymic = curator.Patronymic,
                Phone = curator.Phone,
                VkLink = curator.VkLink,
                TgLink = curator.TgLink,
                MaxLink = curator.MaxLink,
                AvatarPath = curator.AvatarPath,
                University = curator.University.Name
            };
        }
    }
}
