namespace InternshipPlatform.Application.Dtos.ResumeView
{
    public class ResumeViewResponse
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }

        public string? CompanyLogoPath { get; set; }

        public DateTime ViewDate { get; set; }
    }
}
