using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.StudentGroup
{
    public class StudentGroupNotFoundException() : NotFoundException("Учебная группа не найдена");
}
