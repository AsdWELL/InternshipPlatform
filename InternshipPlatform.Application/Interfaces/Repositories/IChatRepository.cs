using InternshipPlatform.Application.Dtos.Chat;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface IChatRepository
    {
        Task<bool> CanStudentAccessChat(int studentId, int chatId);

        Task<bool> CanEmployerAccessChat(int employerId, int chatId);

        Task<bool> CanStudentSendToChat(int studentId, int chatId);

        Task<bool> CanEmployerSendToChat(int employerId, int chatId);

        Task AddChat(Chat chat);

        Task<Chat?> GetChatWithMessagesById(int chatId);

        Task<Chat?> GetActiveChatByStudentAndVacancy(int studentId, int vacancyId);

        Task<Chat?> GetChatByApplicationIdForUpdate(int applicationId);

        Task<List<StudentChatResult>> GetStudentChats(int studentId);

        Task<List<EmployerChatResult>> GetEmployerChats(int employerId);
    }
}
