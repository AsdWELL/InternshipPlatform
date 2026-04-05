using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.Resume
{
    public class InvalidSpecializationException() : BadRequestException("Указанная в резюме специализация не найдена");
}
