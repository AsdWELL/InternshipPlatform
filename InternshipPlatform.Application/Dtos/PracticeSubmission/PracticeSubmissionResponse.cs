namespace InternshipPlatform.Application.Dtos.PracticeSubmission
{
    public class PracticeSubmissionResponse
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? ReviewedAt { get; set; }

        public int? Grade { get; set; }

        public string ReportFileName { get; set; }

        public string? SolutionFileName { get; set; }

        public string? SolutionUrl { get; set; }
    }
}