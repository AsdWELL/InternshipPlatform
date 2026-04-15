using InternshipPlatform.Application.Dtos.JobApplication;
using InternshipPlatform.Application.Dtos.Pagination;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface IJobApplicationService
    {
        Task<int> CreateJobApplicationByStudent(CreateJobApplicaionRequest request);

        Task<int> CreateJobApplicationByEmployer(CreateJobApplicaionRequest request);

        Task<PagedResponse<StudentApplicationResponse>> GetStudentApplications(GetStudentApplicationsParameters request);

        Task<PagedResponse<EmployerApplicationResponse>> GetEmployerApplications(GetEmployerApplicationsParameters request);

        Task UpdateApplicationStatus(UpdateApplicationStatusRequest request);
    }
}
