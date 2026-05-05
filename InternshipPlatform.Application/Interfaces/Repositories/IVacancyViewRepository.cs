using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface IVacancyViewRepository
    {
        Task AddVacancyView(int studentId, int vacancyId);

        Task<List<VacancyView>> GetVacancyViews(int vacancyId);

        Task<List<VacancyView>> GetStudentVacancyViewsHistory(int studentId);
    }
}
