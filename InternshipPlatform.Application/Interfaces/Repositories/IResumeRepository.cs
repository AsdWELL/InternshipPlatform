using InternshipPlatform.Application.Dtos.Pagination;
using InternshipPlatform.Application.Dtos.Resume;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface IResumeRepository
    {
        Task<bool> IsResumeExistsAndActive(int resumeId);
        
        Task<bool> IsStudentOwnsResume(int studentId, int resumeId);

        Task<bool> IsWorkExperienceExists(int workExperienceId);

        Task<bool> IsWorkExperienceBelongsToResume(int resumeId, int workExperienceId);

        Task AddResume(Resume resume);

        Task<List<ResumeResult>> GetStudentResumes(int studentId);

        Task<Resume?> GetResumeById(int id);

        Task<Resume?> GetResumeForUpdate(int id);

        Task<string?> GetStudentEmailByResumeId(int resumeId);

        Task<PagedResult<Resume>> GetRecommendedResumes(int companyId, int pageIndex, int pageSize);

        Task<PagedResult<Resume>> GetRecommendedResumesForVacancy(int vacancyId, int pageIndex, int pageSize);

        Task<PagedResult<Resume>> SearchResumes(SearchResumeParameters parameters);

        Task CopyResume(int resumeId);

        Task DeleteResume(int resumeId);

        Task AddWorkExperience(WorkExperience workExperience);

        Task<WorkExperience?> GetWorkExperienceForUpdate(int id);

        Task DeleteWorkExperience(int workExperienceId);
    }
}
