namespace InternshipPlatform.Application.Dtos.VacancyView
{
    public class EmployerVacancyViewResponse
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public string StudentName { get; set; }

        public string StudentSurname { get; set; }

        public string? StudentPatronymic { get; set; }

        public string? StudentAvatarPath { get; set; }

        public DateTime ViewDate { get; set; }
    }
}
