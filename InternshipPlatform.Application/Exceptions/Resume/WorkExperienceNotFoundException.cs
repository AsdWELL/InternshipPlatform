using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.Resume
{
    public class WorkExperienceNotFoundException() : NotFoundException("Данные об опыте работы не найдены");
}
