using System.Text.Json.Serialization;

namespace InternshipPlatform.Application.Dtos.JobApplication
{
    public class CreateJobApplicationRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }
        
        public int VacancyId { get; set; }

        public int ResumeId { get; set; }

        public string? WelcomeMessage { get; set; }
    }
}
