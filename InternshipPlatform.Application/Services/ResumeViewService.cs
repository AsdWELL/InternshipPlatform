using InternshipPlatform.Application.Dtos.ResumeView;
using InternshipPlatform.Application.Exceptions.Resume;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Mappers;

namespace InternshipPlatform.Application.Services
{
    public class ResumeViewService(
        IResumeViewRepository resumeViewRepository,
        IResumeRepository resumeRepository) : IResumeViewService
    {
        public async Task<List<StudentResumeViewResponse>> GetStudentResumeViews(int studentId, int resumeId)
        {
            if (!await resumeRepository.IsStudentOwnsResume(studentId, resumeId))
                throw new ResumeNotFoundException();

            var views = await resumeViewRepository.GetResumeViews(resumeId);

            return views.Select(v => v.ToStudentResponse()).ToList();
        }

        public async Task<List<EmployerResumeViewResponse>> GetEmployerResumeViewsHistory(int employerId)
        {
            var viewsHistory = await resumeViewRepository.GetEmployerResumeViewsHistory(employerId);
            
            return viewsHistory.Select(v => v.ToEmployerResponse()).ToList();
        }
    }
}
