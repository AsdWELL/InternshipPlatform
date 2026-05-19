namespace InternshipPlatform.Application.Dtos.PracticeSubmission
{
    public class TeacherStudentPracticeItem
    {
        public int StudentPracticeId { get; set; }

        public int StudentId { get; set; }

        public string StudentName { get; set; }

        public string StudentSurname { get; set; }

        public string? StudentPatronymic { get; set; }

        public string GroupName { get; set; }

        public string CompanyName { get; set; }

        public string PracticeOfferTitle { get; set; }

        public string? SubmissionStatus { get; set; }

        public int? Grade { get; set; }
    }
}
