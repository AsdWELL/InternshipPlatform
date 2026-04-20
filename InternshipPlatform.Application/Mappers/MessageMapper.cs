using InternshipPlatform.Application.Dtos.Message;
using InternshipPlatform.Application.Utils;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Mappers
{
    public static class MessageMapper
    {
        public static MessageResponse ToResponse(
            this Message message)
        {
            return new MessageResponse
            {
                Id = message.Id,
                Content = message.Content,
                CreatedAt = message.CreatedAt,
                SenderUserId = message.SenderUserId,
                IsRead = message.IsRead
            };
        }

        public static Message ToDomain(this SendMessageRequest request)
        {
            return new Message
            {
                Content = StringNormalizer.NormalizeRequired(request.Content),
                CreatedAt = DateTime.UtcNow,
                ChatId = request.ChatId,
                SenderUserId = request.SenderUserId,
                IsRead = false
            };
        }
    }
}
