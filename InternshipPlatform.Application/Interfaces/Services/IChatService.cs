using InternshipPlatform.Application.Dtos.Chat;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface IChatService
    {
        Task ThrowIfUserCantAccessChat(int userId, string role, int chatId);
        
        Task<int> GetOrCreateStudentChat(StartStudentChatRequest request);

        Task<int> GetOrCreateEmployerChat(StartEmployerChatRequest request);

        Task<List<StudentChatItem>> GetStudentChats(int studentId);

        Task<List<EmployerChatItem>> GetEmployerChats(int employerId);

        Task<StudentChatMessagesResponse> GetStudentChatMessages(int studentId, int chatId);

        Task<EmployerChatMessagesResponse> GetEmployerChatMessages(int employerId, int chatId);

        Task CloseChatByApplicationId(int userId, string role, int applicationId);
    }
}
