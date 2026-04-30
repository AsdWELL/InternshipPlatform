namespace InternshipPlatform.Domain.Entities
{
    public class VacancyView
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public StudentProfile StudentProfile { get; set; }

        public int VacancyId { get; set; }

        public Vacancy Vacancy { get; set; }

        public DateTime ViewDate { get; set; }
    }
}
