namespace InternshipPlatform.Application.Dtos.PracticeSubmission
{
    public class EmployerStudentPracticeItem
    {
        public int StudentPracticeId { get; set; }

        public int StudentId { get; set; }

        public string StudentName { get; set; }

        public string StudentSurname { get; set; }

        public string? StudentPatronymic { get; set; }

        public string? GroupName { get; set; }

        public int PracticeOfferId { get; set; }

        public string PracticeOfferTitle { get; set; }

        public string? SubmissionStatus { get; set; }
    }
}
