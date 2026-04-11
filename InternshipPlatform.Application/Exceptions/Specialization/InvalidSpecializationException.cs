using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.Specialization
{
    public class InvalidSpecializationException() 
        : ConflictException("SpecializationId", "Указанная специализация не найдена");
}
