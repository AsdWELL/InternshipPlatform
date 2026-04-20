using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.Chat
{
    public class ChatNotFoundException() : NotFoundException("Чат не найден");
}
