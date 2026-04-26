using InternshipPlatform.Application.Dtos.Vacancy;
using InternshipPlatform.Application.Exceptions.Vacancy;
using InternshipPlatform.Application.Interfaces;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Mappers;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Services
{
    public class FavoriteVacancyService(
        IFavoriteVacancyRepository favoriteVacanciesRepository,
        IVacancyRepository vacancyRepository,
        IUnitOfWork unitOfWork) : IFavoriteVacancyService
    {
        public Task<bool> IsVacancyInFavorites(int studentId, int vacancyId)
        {
            return favoriteVacanciesRepository.IsVacancyInStudentFavorites(studentId, vacancyId);
        }

        public async Task AddToFavorites(int studentId, int vacancyId)
        {
            if (!await vacancyRepository.IsVacancyExistsAndActive(vacancyId))
                throw new VacancyNotFoundException();
            
            if (await favoriteVacanciesRepository.IsVacancyInStudentFavorites(studentId, vacancyId))
                throw new VacancyAlreadyInFavoritesException();
            
            await favoriteVacanciesRepository.AddToFavorites(studentId, vacancyId);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveFromFavorites(int studentId, int vacancyId)
        {
            if (!await vacancyRepository.IsVacancyExistsAndActive(vacancyId))
                throw new VacancyNotFoundException();

            if (!await favoriteVacanciesRepository.IsVacancyInStudentFavorites(studentId, vacancyId))
                throw new FavoriteVacancyNotFoundException();

            await favoriteVacanciesRepository.RemoveFromFavorites(studentId, vacancyId);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task<List<VacancyItem>> GetFavoriteVacancies(int studentId)
        {
            var vacancies = await favoriteVacanciesRepository.GetStudentFavoriteVacancies(studentId);

            return vacancies.Select(v => v.ToItem(markAsFavorite: true)).ToList();
        }

        public async Task<List<VacancyItem>> MapToItemAndMarkFavorites(int userId, IEnumerable<Vacancy> vacancies)
        {
            if (!vacancies.Any())
                return [];
            
            var favoriteIds = (await favoriteVacanciesRepository
                .GetStudentFavoriteVacanciesIds(userId, vacancies.Select(v => v.Id)))
                .ToHashSet();

            return vacancies.Select(vacancy => vacancy.ToItem(favoriteIds.Contains(vacancy.Id))).ToList();
        }
    }
}
