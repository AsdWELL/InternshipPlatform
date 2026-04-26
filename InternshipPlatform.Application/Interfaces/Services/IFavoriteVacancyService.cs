using InternshipPlatform.Application.Dtos.Vacancy;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface IFavoriteVacancyService
    {
        Task<bool> IsVacancyInFavorites(int studentId, int vacancyId);
        
        Task AddToFavorites(int studentId, int vacancyId);

        Task RemoveFromFavorites(int studentId, int vacancyId);

        Task<List<VacancyItem>> GetFavoriteVacancies(int studentId);

        Task<List<VacancyItem>> MapToItemAndMarkFavorites(int userId, IEnumerable<Vacancy> vacancies);
    }
}
