using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.Resume
{
    public class InvalidResumeSkillsException() : BadRequestException("В резюме указаны один или несколько несуществующих навыков");
}
