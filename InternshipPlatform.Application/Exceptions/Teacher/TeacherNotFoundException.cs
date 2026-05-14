using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.Teacher
{
    public class TeacherNotFoundException() : NotFoundException("Профиль преподавателя не найден");
}
