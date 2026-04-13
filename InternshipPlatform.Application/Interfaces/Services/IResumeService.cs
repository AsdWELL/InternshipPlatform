using InternshipPlatform.Application.Dtos.Pagination;
using InternshipPlatform.Application.Dtos.Resume;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface IResumeService
    {
        Task<int> CreateResume(CreateResumeRequest request);

        Task<List<ResumeItem>> GetStudentResumes(int studentId);

        Task<ResumeDetails> GetResumeDetails(int resumeId);

        Task<PagedResponse<ResumeItem>> GetRecommendedResumes(GetRecommendedResumesRequest request);

        Task<PagedResponse<ResumeItem>> GetRecommendedResumesForVacancy(GetRecommendedResumesForVacancyRequest request);

        Task<PagedResponse<ResumeItem>> SearchResumes(SearchResumeParameters parameters);

        Task UpdateResume(UpdateResumeRequest request);

        Task<int> AddWorkExperience(AddWorkExperienceRequest request);

        Task UpdateWorkExperience(UpdateWorkExperienceRequest request);

        Task DeleteWorkExperience(int workExperienceId);

        Task DeleteResume(int studentId, int resumeId);

        Task CopyResume(int studentId, int resumeId);
    }
}
