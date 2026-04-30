namespace InternshipPlatform.Application.Dtos.JobApplication
{
    public class EmployerApplicationResponse
    {
        public int Id { get; set; }

        public int ResumeId { get; set; }

        public string SpecializationName { get; set; }

        public int StudentId { get; set; }
        
        public string StudentName { get; set; }

        public string StudentSurname { get; set; }

        public string? StudentPatronymic { get; set; }

        public string? StudentAvatarPath { get; set; }

        public DateOnly LastStatusDate { get; set; }

        public string ApplicationStatus { get; set; }

        public int ChatId { get; set; }
    }
}
