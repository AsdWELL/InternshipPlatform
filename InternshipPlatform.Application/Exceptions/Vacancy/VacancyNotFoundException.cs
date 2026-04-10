using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.Vacancy
{
    public class VacancyNotFoundException() : NotFoundException("Вакансия не найденна");
}
