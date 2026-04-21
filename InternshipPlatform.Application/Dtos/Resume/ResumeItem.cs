namespace InternshipPlatform.Application.Dtos.Resume
{
    public class ResumeItem
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public string StudentName { get; set; }

        public string StudentSurname { get; set; }

        public string? StudentPatronymic { get; set; }

        public DateOnly LastUpdateDate { get; set; }

        public int DesiredSalary { get; set; }

        public string? Region { get; set; }

        public string SpecializationName { get; set; }

        public List<WorkExperienceItem> WorkExperiences { get; set; }

        public int TotalWorkExperienceMonths { get; set; }
    }
}
