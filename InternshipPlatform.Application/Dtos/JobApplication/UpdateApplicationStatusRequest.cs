using InternshipPlatform.Application.Values;
using System.Text.Json.Serialization;

namespace InternshipPlatform.Application.Dtos.JobApplication
{
    public class UpdateApplicationStatusRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }

        [JsonIgnore]
        public string? Role { get; set; }

        [JsonIgnore]
        public int ApplicationId { get; set; }

        public JobApplicationStatuses ApplicationStatus { get; set; }
    }
}
