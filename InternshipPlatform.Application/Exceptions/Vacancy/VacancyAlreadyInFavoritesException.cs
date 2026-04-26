using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.Vacancy
{
    public class VacancyAlreadyInFavoritesException() : ConflictException("VacancyId", "Вакансия уже добавлена в избранное");
}
