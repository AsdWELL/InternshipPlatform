using InternshipPlatform.Application.Dtos.Pagination;
using InternshipPlatform.Application.Dtos.Vacancy;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface IVacancyRepository
    {
        Task<bool> IsEmployerOwnsVacancy(int employerId, int vacancyId);
        
        Task AddVacancy(Vacancy vacancy);

        Task<List<Vacancy>> GetCompanyVacancies(int companyId);

        Task<Vacancy?> GetVacancyById(int vacancyId);

        Task<Vacancy?> GetVacancyForUpdate(int vacancyId);

        Task<PagedResult<Vacancy>> GetRecommendedVacancies(int studentId, int pageIndex, int pageSize);

        Task<PagedResult<Vacancy>> GetRecommendedVacanciesForResume(int resumeId, int pageIndex, int pageSize);

        Task<PagedResult<Vacancy>> SearchVacancies(SearchVacancyParameters parameters);

        Task DeleteVacancy(int vacancyId);
    }
}
