using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.StudentProfile
{
    public class StudentProfileNotFoundException() : NotFoundException("Профиль студента не найден");
}
