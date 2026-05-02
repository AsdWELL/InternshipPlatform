using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.University
{
    public class InvalidUniversityException() : ConflictException("UniversityId", "Указанный университет не найден");
}
