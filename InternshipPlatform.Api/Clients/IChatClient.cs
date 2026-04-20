using InternshipPlatform.Application.Dtos.Message;

namespace InternshipPlatform.Api.Clients
{
    public interface IChatClient
    {
        Task ReceiveMessage(MessageResponse message);

        Task ChatRead(int chatId);

        Task ChatUpdate(int chatId);
    }
}
