using InternshipPlatform.Application.Dtos.VacancyView;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface IVacancyViewService
    {
        Task<List<EmployerVacancyViewResponse>> GetEmployerVacancyViews(int employerId, int vacancyId);

        Task<List<StudentVacancyViewResponse>> GetStudentVacancyViewsHistory(int studentId);
    }
}
