using InternshipPlatform.Application.Dtos.Pagination;
using InternshipPlatform.Application.Dtos.Vacancy;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface IVacancyService
    {
        Task<int> CreateVacancy(CreateVacancyRequest request);

        Task<List<VacancyItem>> GetEmployerVacancies(int employerId);

        Task<VacancyDetails> GetVacancyDetails(int vacancyId);

        Task<PagedResponse<VacancyItem>> GetRecommendedVacancies(GetRecommendedVacanciesRequest request);

        Task<PagedResponse<VacancyItem>> GetRecommendedVacanciesForResume(GetRecommendedVacanciesForResumeRequest request);

        Task<PagedResponse<VacancyItem>> SearchVacancies(SearchVacancyParameters parameters);

        Task UpdateVacancy(UpdateVacancyRequest request);

        Task DeleteVacancy(int employerId, int vacancyId);
    }
}
