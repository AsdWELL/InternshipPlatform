using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class FavoriteVacancyRepository(InternshipPlatformContext context) : IFavoriteVacancyRepository
    {
        public async Task<bool> IsVacancyInStudentFavorites(int studentId, int vacancyId)
        {
            return await context.FavoriteVacancies
                .AsNoTracking()
                .AnyAsync(fv => fv.StudentId == studentId && fv.VacancyId == vacancyId);
        }

        public async Task AddToFavorites(int studentId, int vacancyId)
        {
            await context.FavoriteVacancies.AddAsync(new FavoriteVacancy
            {
                StudentId = studentId,
                VacancyId = vacancyId
            });
        }

        public async Task RemoveFromFavorites(int studentId, int vacancyId)
        {
            var favoriteVacancy = await context.FavoriteVacancies
                .FirstOrDefaultAsync(fv => fv.StudentId == studentId && fv.VacancyId == vacancyId);

            if (favoriteVacancy is null)
                return;

            context.FavoriteVacancies.Remove(favoriteVacancy);
        }

        public async Task<List<Vacancy>> GetStudentFavoriteVacancies(int studentId)
        {
            return await context.FavoriteVacancies
                .AsNoTracking()
                .Where(fv => fv.StudentId == studentId)
                .Include(fv => fv.Vacancy)
                    .ThenInclude(v => v.Company)
                .Include(fv => fv.Vacancy)
                    .ThenInclude(v => v.Specialization)
                .Select(fv => fv.Vacancy)
                .ToListAsync();
        }

        public async Task<List<int>> GetStudentFavoriteVacanciesIds(int studentId, IEnumerable<int> vacanciesId)
        {
            var vacancyIdsList = vacanciesId.Distinct().ToList();

            if (vacancyIdsList.Count == 0)
                return [];

            return await context.FavoriteVacancies
                .AsNoTracking()
                .Where(fv => fv.StudentId == studentId && vacancyIdsList.Contains(fv.VacancyId))
                .Select(fv => fv.VacancyId)
                .ToListAsync();
        }
    }
}
