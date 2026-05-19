using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.StudentPractice
{
    public class StudentPracticeNotFoundException() : NotFoundException("Практика студента не найдена");
}
