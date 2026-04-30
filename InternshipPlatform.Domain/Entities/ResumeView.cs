namespace InternshipPlatform.Domain.Entities
{
    public class ResumeView
    {
        public int Id { get; set; }

        public int ResumeId { get; set; }

        public Resume Resume { get; set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public DateTime ViewDate { get; set; }
    }
}
