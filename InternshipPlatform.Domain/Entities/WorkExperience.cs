namespace InternshipPlatform.Domain.Entities
{
    public class WorkExperience
    {
        public int Id { get; set; }

        public int ResumeId { get; set; }

        public string CompanyName { get; set; }

        public string Profession { get; set; }

        public DateOnly StartDateWork { get; set; }

        public DateOnly? EndDateWork { get; set; }

        public string? WorkDescription { get; set; }
    }
}
