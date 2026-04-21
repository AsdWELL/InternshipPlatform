namespace InternshipPlatform.Domain.Entities
{
    public class Vacancy
    {
        public int Id { get; set; }

        public string Title { get; set; }
        
        public string? Description { get; set; }

        public int SalaryFrom { get; set; }

        public int SalaryTo { get; set; }

        public bool IsRemote { get; set; }

        public string? Region { get; set; }

        public bool IsActive { get; set; }

        public int ViewsCount { get; set; }

        public int MinWorkExperienceYears { get; set; }

        public int SpecializationId { get; set; }

        public Specialization Specialization { get; set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public List<Skill> Skills { get; set; } = [];

        public List<JobApplication> Applications { get; set; }
    }
}
