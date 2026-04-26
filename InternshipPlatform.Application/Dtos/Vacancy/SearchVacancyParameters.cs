using InternshipPlatform.Application.Dtos.Pagination;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace InternshipPlatform.Application.Dtos.Vacancy
{
    public class SearchVacancyParameters : PageRequestParameters
    {
        [BindNever]
        [JsonIgnore]
        public int StudentId { get; set; }
        
        public string? Search { get; set; }

        public bool SearchInTitle { get; set; }

        public bool SearchInDescription { get; set; }

        public bool SearchInCompanyName { get; set; }

        public int? SalaryFrom { get; set; }

        public int? SalaryTo { get; set; }

        public bool? IsRemote { get; set; }

        public string? Region { get; set; }

        public int? MinWorkExperienceYears { get; set; }

        public int? MaxWorkExperienceYears { get; set; }

        public int? SpecializationId { get; set; }

        public List<int>? SkillIds { get; set; }
    }
}
