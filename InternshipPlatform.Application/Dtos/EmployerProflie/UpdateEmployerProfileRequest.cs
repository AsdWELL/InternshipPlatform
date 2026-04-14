using System.Text.Json.Serialization;

namespace InternshipPlatform.Application.Dtos.EmployerProflie
{
    public class UpdateEmployerProfileRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? PasswordConfirm { get; set; }
    }
}
