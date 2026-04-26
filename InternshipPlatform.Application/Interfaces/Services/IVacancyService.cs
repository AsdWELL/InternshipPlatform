using InternshipPlatform.Application.Dtos.Pagination;
using InternshipPlatform.Application.Dtos.Vacancy;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface IVacancyService
    {
        Task<int> CreateVacancy(CreateVacancyRequest request);

        Task<List<VacancyOwnerItem>> GetEmployerVacancies(int employerId);

        Task<List<VacancyItem>> GetCompanyVacancies(int userId, int companyId);

        Task<VacancyDetails> GetVacancyDetailsForStudent(int studentId, int vacancyId);

        Task<VacancyOwnerDetails> GetVacancyDetailsForOwner(int employerId, int vacancyId);

        Task<PagedResponse<VacancyItem>> GetRecommendedVacancies(GetRecommendedVacanciesRequest request);

        Task<PagedResponse<VacancyItem>> GetRecommendedVacanciesForResume(GetRecommendedVacanciesForResumeRequest request);

        Task<PagedResponse<VacancyItem>> SearchVacancies(SearchVacancyParameters parameters);

        Task UpdateVacancy(UpdateVacancyRequest request);

        Task DeleteVacancy(int employerId, int vacancyId);
    }
}
