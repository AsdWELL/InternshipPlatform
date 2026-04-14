using System.Text.Json.Serialization;

namespace InternshipPlatform.Application.Dtos.Resume
{
    public class AddWorkExperienceRequest
    {
        [JsonIgnore]
        public int StudentId { get; set; }

        [JsonIgnore]
        public int ResumeId { get; set; }

        public string CompanyName { get; set; }

        public string Profession { get; set; }

        public DateOnly StartDateWork { get; set; }

        public DateOnly? EndDateWork { get; set; }

        public string? WorkDescription { get; set; }

    }
}
