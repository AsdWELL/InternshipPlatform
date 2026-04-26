using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface IFavoriteVacancyRepository
    {
        Task<bool> IsVacancyInStudentFavorites(int studentId, int vacancyId);
        
        Task AddToFavorites(int studentId, int vacancyId);

        Task RemoveFromFavorites(int studentId, int vacancyId);

        Task<List<Vacancy>> GetStudentFavoriteVacancies(int studentId);

        Task<List<int>> GetStudentFavoriteVacanciesIds(int studentId, IEnumerable<int> vacanciesId);
    }
}
