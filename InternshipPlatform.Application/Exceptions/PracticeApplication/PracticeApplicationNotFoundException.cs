using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.PracticeApplication
{
    public class PracticeApplicationNotFoundException() : NotFoundException("Заявка на практику не найдена");
}