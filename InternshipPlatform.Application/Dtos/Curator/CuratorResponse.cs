namespace InternshipPlatform.Application.Dtos.Curator
{
    public class CuratorResponse
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public bool IsVerified { get; set; }

        public string University { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string? Patronymic { get; set; }

        public string? Phone { get; set; }

        public string? VkLink { get; set; }

        public string? TgLink { get; set; }

        public string? MaxLink { get; set; }

        public string? AvatarPath { get; set; }
    }
}
