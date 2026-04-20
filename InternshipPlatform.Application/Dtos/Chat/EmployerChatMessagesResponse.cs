using InternshipPlatform.Application.Dtos.Message;

namespace InternshipPlatform.Application.Dtos.Chat
{
    public class EmployerChatMessagesResponse
    {
        public int Id { get; set; }

        public int VacancyId { get; set; }

        public string VacancyTitle { get; set; }

        public int StudentId { get; set; }

        public string StudentName { get; set; }

        public string StudentSurname { get; set; }

        public string? StudentPatronymic { get; set; }

        public string? StudentAvatarPath { get; set; }

        public string? SpecializationName { get; set; }

        public List<MessageResponse> Messages { get; set; }

        public bool IsClosed { get; set; }
    }
}
