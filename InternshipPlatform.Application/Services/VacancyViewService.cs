using InternshipPlatform.Application.Dtos.VacancyView;
using InternshipPlatform.Application.Exceptions.Vacancy;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Mappers;

namespace InternshipPlatform.Application.Services
{
    public class VacancyViewService(
        IVacancyViewRepository vacancyViewRepository,
        IVacancyRepository vacancyRepository) : IVacancyViewService
    {
        public async Task<List<EmployerVacancyViewResponse>> GetEmployerVacancyViews(int employerId, int vacancyId)
        {
            if (!await vacancyRepository.IsEmployerOwnsVacancy(employerId, vacancyId))
                throw new VacancyNotFoundException();

            var views = await vacancyViewRepository.GetVacancyViews(vacancyId);

            return views.Select(v => v.ToEmployerResponse()).ToList();
        }

        public async Task<List<StudentVacancyViewResponse>> GetStudentVacancyViewsHistory(int studentId)
        {
            var viewHistory = await vacancyViewRepository.GetStudentVacancyViewsHistory(studentId);

            return viewHistory.Select(v => v.ToStudentResponse()).ToList();
        }
    }
}
