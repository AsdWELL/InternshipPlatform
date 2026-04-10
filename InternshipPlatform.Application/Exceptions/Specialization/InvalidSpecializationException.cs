using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.Specialization
{
    public class InvalidSpecializationException() : BadRequestException("Указанная специализация не найдена");
}
