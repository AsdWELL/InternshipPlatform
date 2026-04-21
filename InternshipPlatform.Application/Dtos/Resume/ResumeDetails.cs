using InternshipPlatform.Application.Dtos.StudentProfile;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Dtos.Resume
{
    public class ResumeDetails
    {
        public int Id { get; set; }

        public DateOnly LastUpdateDate { get; set; }

        public StudentProfileResponse StudentProfile { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public int DesiredSalary { get; set; }

        public string? Region { get; set; }

        public Specialization Specialization { get; set; }

        public int TotalWorkExperienceMonths { get; set; }

        public List<WorkExperienceDetails> WorkExperiences { get; set; }

        public List<Skill> Skills { get; set; }
    }
}
