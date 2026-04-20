using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Dtos.Chat
{
    public class EmployerChatResult
    {
        public int Id { get; set; }

        public int VacancyId { get; set; }

        public string VacancyTitle { get; set; }

        public int StudentId { get; set; }

        public string StudentName { get; set; }

        public string StudentSurname { get; set; }

        public string? StudentPatronymic { get; set; }

        public string? StudentAvatarPath { get; set; }

        public string? SpecializationName {get; set;}

        public int UnreadMessagesCount { get; set; }

        public Domain.Entities.Message? LastMessage { get; set; }

        public bool IsClosed { get; set; }
    }
}
