using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.StudentGroupApplication
{
    public class StudentGroupApplicationNotFoundException() : NotFoundException("Заявка на добавление в группу не найдена");
}
