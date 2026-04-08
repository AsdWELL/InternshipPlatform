using InternshipPlatform.Application.Dtos.Pagination;

namespace InternshipPlatform.Application.Dtos.Resume
{
    public enum ResumeUpdatedDateFilter
    {
        AllTime,
        Last24Hours,
        Last3Days,
        LastWeek,
        LastMonth,
        LastYear,
        CustomInterval
    }

    public class SearchResumeParameters : PageRequestParameters
    {
        public int? SalaryFrom { get; set; }

        public int? SalaryTo { get; set; }

        public string? Region { get; set; }

        public int? SpecializationId { get; set; }

        public int? AgeFrom { get; set; }

        public int? AgeTo { get; set; }

        public List<int>? SkillIds { get; set; }

        public int? MinWorkExperienceYears { get; set; }

        public int? MaxWorkExperienceYears { get; set; }

        public ResumeUpdatedDateFilter? ResumeUpdatedDateFilter { get; set; }

        public DateOnly? UpdatedFrom { get; set; }

        public DateOnly? UpdatedTo { get; set; }
    }
}
