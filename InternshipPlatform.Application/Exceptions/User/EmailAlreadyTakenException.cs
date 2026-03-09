using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.User
{
    public class EmailAlreadyTakenException(string email) : ConflictException($"{email} уже используется");
}
