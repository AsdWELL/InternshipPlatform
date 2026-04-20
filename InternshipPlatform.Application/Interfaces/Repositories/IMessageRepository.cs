using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface IMessageRepository
    {
        Task AddMessage(Message message);

        Task MarkChatAsRead(int userId, int chatId);
    }
}
