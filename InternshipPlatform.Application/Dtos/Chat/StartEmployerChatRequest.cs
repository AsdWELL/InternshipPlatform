using System.Text.Json.Serialization;

namespace InternshipPlatform.Application.Dtos.Chat
{
    public class StartEmployerChatRequest
    {
        [JsonIgnore]
        public int EmployerId { get; set; }

        public int StudentId { get; set; }

        public int VacancyId { get; set; }

        public string? Message { get; set; }
    }
}
