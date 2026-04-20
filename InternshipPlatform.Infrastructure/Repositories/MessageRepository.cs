using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class MessageRepository(InternshipPlatformContext context) : IMessageRepository
    {
        public async Task AddMessage(Message message)
        {
            await context.Messages.AddAsync(message);
        }

        public async Task MarkChatAsRead(int userId, int chatId)
        {
            await context.Messages
                .Where(m => m.ChatId == chatId && m.SenderUserId != userId)
                .ForEachAsync(m => m.IsRead = true);
        }
    }
}
