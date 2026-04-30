using InternshipPlatform.Application.Dtos.ResumeView;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface IResumeViewService
    {
        Task<List<ResumeViewResponse>> GetResumeViews(int studentId, int resumeId);
    }
}
