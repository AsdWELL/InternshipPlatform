using InternshipPlatform.Application.Dtos.Message;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface IMessageService
    {
        Task<MessageResponse> SendMessage(SendMessageRequest request);

        Task MarkStudentChatAsRead(int studentId, int chatId);

        Task MarkEmployerChatAsRead(int employerId, int chatId);
    }
}
