using InternshipPlatform.Application.Dtos.Curator;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Mappers
{
    public static class CuratorMapper
    {
        public static CuratorResponse ToResponse(this Curator curator)
        {
            return new CuratorResponse
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
                IsVerified = curator.User.IsVerified,
                University = curator.University.Name
            };
        }
    }
}
