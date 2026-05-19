using Microsoft.AspNetCore.Http;

namespace InternshipPlatform.Application.Dtos.PracticeSubmission
{
    public class UploadPracticeSubmissionRequest
    {
        public int StudentId { get; set; }

        public IFormFile ReportFile { get; set; }

        public IFormFile? SolutionFile { get; set; }

        public string? SolutionUrl { get; set; }
    }
}