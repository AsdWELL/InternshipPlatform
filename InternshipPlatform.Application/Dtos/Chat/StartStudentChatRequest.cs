using System.Text.Json.Serialization;

namespace InternshipPlatform.Application.Dtos.Chat
{
    public class StartStudentChatRequest
    {
        [JsonIgnore]
        public int StudentId { get; set; }

        public int VacancyId { get; set; }

        public string Message { get; set; }
    }
}
