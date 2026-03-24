using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.Company
{
    public class InnAlreadyTakenException(string propertyName, string inn) 
        : ConflictException(propertyName, $"ИНН {inn} уже используется в системе");
}
