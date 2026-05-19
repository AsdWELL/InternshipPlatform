namespace InternshipPlatform.Application.Dtos.PracticeSubmission
{
    public class EmployerStudentPracticeDetails
    {
        public int StudentPracticeId { get; set; }

        public int StudentId { get; set; }

        public string StudentName { get; set; }

        public string StudentSurname { get; set; }

        public string? StudentPatronymic { get; set; }

        public string StudentEmail { get; set; }

        public string? GroupName { get; set; }

        public int PracticeOfferId { get; set; }

        public string PracticeOfferTitle { get; set; }

        public int PracticePeriodId { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public PracticeSubmissionResponse? Submission { get; set; }
    }
}
