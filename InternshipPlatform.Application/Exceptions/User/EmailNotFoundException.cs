using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.User
{
    public class EmailNotFoundException(string email) 
        : NotFoundException($"Пользователь с электронной почтой {email} не найден");
}
