using System.Text.Json.Serialization;

namespace InternshipPlatform.Application.Dtos.Vacancy
{
    public class UpdateVacancyRequest
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public int EmployerId { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public int? SalaryFrom { get; set; }

        public int? SalaryTo { get; set; }

        public bool? IsRemote { get; set; }

        public string? Region { get; set; }

        public bool? IsActive { get; set; }

        public int? MinWorkExperienceYears { get; set; }

        public int? SpecializationId { get; set; }

        public List<int>? SkillIds { get; set; }
    }
}
