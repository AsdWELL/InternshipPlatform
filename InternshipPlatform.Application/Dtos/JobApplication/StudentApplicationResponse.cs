namespace InternshipPlatform.Application.Dtos.JobApplication
{
    public class StudentApplicationResponse
    {
        public int Id { get; set; }

        public int VacancyId { get; set; }

        public string VacancyTitle { get; set; }

        public int CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string? CompanyLogoPath { get; set; }

        public DateOnly LastStatusDate { get; set; }

        public string ApplicationStatus { get; set; }

        public int ChatId { get; set; }
    }
}
