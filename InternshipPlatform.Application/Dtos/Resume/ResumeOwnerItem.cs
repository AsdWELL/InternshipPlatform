namespace InternshipPlatform.Application.Dtos.Resume
{
    public class ResumeOwnerItem
    {
        public int Id { get; set; }

        public DateOnly LastUpdateDate { get; set; }

        public bool IsActive { get; set; }

        public int DesiredSalary { get; set; }

        public string? Region { get; set; }

        public string SpecializationName { get; set; }

        public List<WorkExperienceItem> WorkExperiences { get; set; }

        public int TotalWorkExperienceMonths { get; set; }

        public int ApplicationsCount { get; set; }
    }
}
