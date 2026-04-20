namespace InternshipPlatform.Domain.Entities
{
    public class Chat
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public int VacancyId { get; set; }

        public Vacancy Vacancy { get; set; }

        public int StudentId { get; set; }

        public StudentProfile StudentProfile { get; set; }

        public List<Message> Messages { get; set; } = [];

        public JobApplication? Application { get; set; }

        public bool IsClosed { get; set; }
    }
}
