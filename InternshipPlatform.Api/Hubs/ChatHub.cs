using InternshipPlatform.Api.Clients;
using InternshipPlatform.Application.Dtos.Message;
using InternshipPlatform.Application.Exceptions.Chat;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace InternshipPlatform.Api.Hubs
{
    [Authorize]
    public class ChatHub(
        IMessageService messageService,
        IChatService chatService) : Hub<IChatClient>
    {
        private int UserId => Convert.ToInt32(Context.User!.FindFirstValue(ClaimTypes.NameIdentifier)!);

        private string Role => Context.User!.FindFirstValue(ClaimTypes.Role)!;

        private static string GetChatGroup(int chatId) => $"chat_{chatId}";

        public async Task JoinChat(int chatId)
        {
            try
            {
                await chatService.ThrowIfUserCantAccessChat(UserId, Role, chatId);

                await Groups.AddToGroupAsync(Context.ConnectionId, GetChatGroup(chatId));
            }
            catch (Exception ex)
            {
                throw new HubException(ex.Message);
            }
        }

        public async Task LeaveChat(int chatId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetChatGroup(chatId));
        }

        public async Task SendMessage(SendMessageRequest request)
        {
            if (string.IsNullOrEmpty(request.Content))
                throw new HubException("Пустое сообщение");

            request.SenderUserId = UserId;
            request.Role = Role;

            MessageResponse message;

            try
            {
                message = await messageService.SendMessage(request);
            }
            catch (Exception ex)
            {
                throw new HubException(ex.Message);
            }

            await Clients.Group(GetChatGroup(request.ChatId)).ReceiveMessage(message);

            await Clients.Group(GetChatGroup(request.ChatId)).ChatUpdate(request.ChatId);
        }

        public async Task MarkChatAsRead(int chatId)
        {
            try
            {
                if (Role == Roles.Student)
                    await messageService.MarkStudentChatAsRead(UserId, chatId);
                else if (Role == Roles.Employer)
                    await messageService.MarkEmployerChatAsRead(UserId, chatId);
                else
                    throw new ChatNotFoundException();
            }
            catch (Exception ex)
            {
                throw new HubException(ex.Message);
            }
        }
    }
}
