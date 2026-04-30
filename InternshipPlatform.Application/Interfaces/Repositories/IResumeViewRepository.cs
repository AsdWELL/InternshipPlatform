using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface IResumeViewRepository
    {
        Task AddResumeView(int companyId, int resumeId);

        Task<List<ResumeView>> GetResumeViews(int resumeId);

        Task<Dictionary<int, int>> GetResumesViewsCount(int studentId);

        Task<List<ResumeView>> GetEmployerResumeViewsHistory(int employerId);
    }
}
