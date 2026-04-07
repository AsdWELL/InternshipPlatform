using System.Text.Json.Serialization;

namespace InternshipPlatform.Application.Dtos.Resume
{
    public class CreateResumeRequest
    {
        public string? Description { get; set; }

        public int? DesiredSalary { get; set; }

        public string? Region { get; set; }

        public int SpecializationId { get; set; }

        [JsonIgnore]
        public int StudentId { get; set; }

        public List<int> SkillIds { get; set; }
    }
}
