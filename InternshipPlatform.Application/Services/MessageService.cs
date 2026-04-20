using InternshipPlatform.Application.Dtos.Message;
using InternshipPlatform.Application.Exceptions.Chat;
using InternshipPlatform.Application.Interfaces;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Mappers;
using InternshipPlatform.Application.Values;

namespace InternshipPlatform.Application.Services
{
    public class MessageService(
        IMessageRepository messageRepository,
        IChatRepository chatRepository,
        IUnitOfWork unitOfWork) : IMessageService
    {
        public async Task<MessageResponse> SendMessage(SendMessageRequest request)
        {
            var canUserSend = request.Role switch
            {
                Roles.Student => await chatRepository.CanStudentSendToChat(request.SenderUserId, request.ChatId),
                Roles.Employer => await chatRepository.CanEmployerSendToChat(request.SenderUserId, request.ChatId),
                _ => false
            };

            if (!canUserSend)
                throw new ChatNotFoundException();
            
            var message = request.ToDomain();
            await messageRepository.AddMessage(message);

            await unitOfWork.SaveChangesAsync();

            return message.ToResponse();
        }

        public async Task MarkEmployerChatAsRead(int employerId, int chatId)
        {
            if (!await chatRepository.CanEmployerAccessChat(employerId, chatId))
                throw new ChatNotFoundException();

            await messageRepository.MarkChatAsRead(employerId, chatId);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task MarkStudentChatAsRead(int studentId, int chatId)
        {
            if (!await chatRepository.CanStudentAccessChat(studentId, chatId))
                throw new ChatNotFoundException();

            await messageRepository.MarkChatAsRead(studentId, chatId);

            await unitOfWork.SaveChangesAsync();
        }
    }
}
