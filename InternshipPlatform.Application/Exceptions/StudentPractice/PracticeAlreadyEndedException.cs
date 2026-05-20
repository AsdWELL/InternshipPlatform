using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.StudentPractice
{
    public class PracticeAlreadyEndedException() : BadRequestException("Практика уже завершена");
}
