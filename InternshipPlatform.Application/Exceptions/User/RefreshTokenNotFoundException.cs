using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.User
{
    public class RefreshTokenNotFoundException() : NotFoundException("Пользователь с указанным токеном не найден");
}
