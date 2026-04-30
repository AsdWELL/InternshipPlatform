namespace InternshipPlatform.Application.Dtos.ResumeView
{
    public class EmployerResumeViewResponse
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public string StudentName { get; set; }

        public string StudentSurname { get; set; }

        public string? StudentPatronymic { get; set; }

        public string? StudentAvatarPath { get; set; }

        public int ResumeId { get; set; }

        public string SpecializationName { get; set; }

        public DateTime ViewDate { get; set; }
    }
}
