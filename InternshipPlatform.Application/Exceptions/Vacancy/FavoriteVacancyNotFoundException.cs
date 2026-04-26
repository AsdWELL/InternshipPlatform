using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.Vacancy
{
    public class FavoriteVacancyNotFoundException() : NotFoundException("Вакансия не найдена в избранном");
}
