using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.Company
{
    public class InnAlreadyTakenException(string inn) 
        : ConflictException("Inn", $"ИНН {inn} уже используется в системе");
}
