using InternshipPlatform.Application.Dtos.Pagination;
using InternshipPlatform.Application.Dtos.Resume;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface IResumeRepository
    {
        Task<bool> IsStudentOwnsResume(int studentId, int resumeId);

        Task<bool> IsWorkExperienceExists(int workExperienceId);

        Task<bool> IsWorkExperienceBelongsToResume(int resumeId, int workExperienceId);

        Task AddResume(Resume resume);

        Task<List<Resume>> GetStudentResumes(int studentId);

        Task<Resume?> GetResumeById(int id);

        Task<PagedResult<Resume>> SearchResumes(SearchResumeParameters parameters);

        Task UpdateResume(Resume resume);

        Task CopyResume(int resumeId);

        Task DeleteResume(int resumeId);

        Task AddWorkExperience(WorkExperience workExperience);

        Task UpdateWorkExperience(WorkExperience workExperience);

        Task DeleteWorkExperience(int workExperienceId);
    }
}
