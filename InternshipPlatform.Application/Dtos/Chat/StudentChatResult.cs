namespace InternshipPlatform.Application.Dtos.Chat
{
    public class StudentChatResult
    {
        public int Id { get; set; }

        public int VacancyId { get; set; }

        public string VacancyTitle { get; set; }

        public int CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string? CompanyLogoPath { get; set; }

        public int UnreadMessagesCount { get; set; }

        public Domain.Entities.Message? LastMessage { get; set; }

        public bool IsClosed { get; set; }
    }
}
