using System.Text.Json.Serialization;

namespace InternshipPlatform.Application.Dtos.Curator
{
    public class UpdateCuratorRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }

        public string? Email { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public string? Password { get; set; }

        public string? PasswordConfirm { get; set; }

        public string? Patronymic { get; set; }

        public string? Phone { get; set; }

        public string? VkLink { get; set; }

        public string? TgLink { get; set; }

        public string? MaxLink { get; set; }
    }
}
