namespace InternshipPlatform.Domain.Entities
{
    public class Resume
    {
        public int Id { get; set; }

        public DateOnly LastUpdateDate { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public int DesiredSalary { get; set; }

        public string? Region { get; set; }

        public int SpecializationId { get; set; }

        public Specialization Specialization { get; set; }

        public int StudentId { get; set; }

        public StudentProfile StudentProfile { get; set; }

        public List<Skill> Skills { get; set; } = [];

        public List<WorkExperience> WorkExperiences { get; set; } = [];

        public List<JobApplication> Applications { get; set; } = [];
    }
}
