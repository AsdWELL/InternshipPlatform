using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.User
{
    public class EmailAlreadyTakenException(string propertyName, string email) 
        : ConflictException(propertyName, $"{email} уже используется");
}
