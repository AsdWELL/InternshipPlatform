using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.Resume
{
    public class ResumeNotFoundException() : NotFoundException("Резюме не найдено");
}
