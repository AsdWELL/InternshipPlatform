using InternshipPlatform.Application.Dtos.PracticeMaterial;
using InternshipPlatform.Application.Dtos.SubmissionComment;

namespace InternshipPlatform.Application.Dtos.PracticeSubmission
{
    public class StudentCurrentPracticeResponse
    {
        public int StudentPracticeId { get; set; }

        public int PracticeOfferId { get; set; }

        public string PracticeOfferTitle { get; set; }

        public string? PracticeOfferDescription { get; set; }

        public int CompanyId { get; set; }

        public string CompanyName { get; set; }

        public int PracticePeriodId { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public int SupervisorId { get; set; }

        public string SupervisorName { get; set; }

        public string SupervisorSurname { get; set; }

        public string? SupervisorPatronymic { get; set; }

        public List<PracticeMaterialResponse> Materials { get; set; } = [];

        public PracticeSubmissionResponse? Submission { get; set; }

        public List<SubmissionCommentResponse>? Comments { get; set; }
    }
}
