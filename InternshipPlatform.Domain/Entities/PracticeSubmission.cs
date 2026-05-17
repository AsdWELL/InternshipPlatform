namespace InternshipPlatform.Domain.Entities
{
    public class PracticeSubmission
    {
        public int Id { get; set; }

        public string ReportFilePath { get; set; }

        public string? SolutionPath { get; set; }

        public string? SolutionUrl { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? ReviewedAt { get; set; }

        public int? Grade { get; set; }

        public int StudentPracticeId { get; set; }

        public StudentPractice StudentPractice { get; set; }

        public int StatusId { get; set; }

        public PracticeSubmissionStatus Status { get; set; }
    }
}
