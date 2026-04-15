namespace InternshipPlatform.Domain.Entities
{
    public class JobApplication
    {
        public int Id { get; set; }

        public int VacancyId { get; set; }

        public Vacancy Vacancy { get; set; }

        public int ResumeId { get; set; }

        public Resume Resume { get; set; }

        public DateOnly LastStatusDate { get; set; }

        public int ApplicationStatusId { get; set; }

        public JobApplicationStatus ApplicationStatus { get; set; }
    }
}
