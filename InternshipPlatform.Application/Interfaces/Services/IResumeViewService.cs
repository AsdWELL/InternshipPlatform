using InternshipPlatform.Application.Dtos.ResumeView;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface IResumeViewService
    {
        Task<List<StudentResumeViewResponse>> GetStudentResumeViews(int studentId, int resumeId);

        Task<List<EmployerResumeViewResponse>> GetEmployerResumeViewsHistory(int employerId);
    }
}
