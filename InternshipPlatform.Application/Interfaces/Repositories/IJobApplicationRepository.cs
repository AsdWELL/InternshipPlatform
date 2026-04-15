using InternshipPlatform.Application.Dtos.JobApplication;
using InternshipPlatform.Application.Dtos.Pagination;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface IJobApplicationRepository
    {
        Task AddJobApplication(JobApplication application);

        Task<JobApplication?> GetStudentApplicationForUpdate(int studentId, int applicationId);

        Task<JobApplication?> GetEmployerApplicationForUpdate(int employerId, int applicationId);

        Task<PagedResult<JobApplication>> GetStudentApplications(GetStudentApplicationsParameters request);

        Task<PagedResult<JobApplication>> GetEmployerApplications(GetEmployerApplicationsParameters request);
    }
}
