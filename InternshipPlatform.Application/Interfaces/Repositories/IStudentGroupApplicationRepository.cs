using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface IStudentGroupApplicationRepository
    {
        Task<bool> IsStudentHasGroupApplication(int studentId);

        Task<bool> IsStudentOwnsGroupApplication(int studentId, int applicationId);

        Task AddStudentGroupApplication(StudentGroupApplication application);

        Task<StudentGroupApplication?> GetStudentGroupApplication(int studentId);

        Task<StudentGroupApplication?> GetCuratorGroupApplicationForUpdate(int curatorId, int applicationId);

        Task<List<StudentGroupApplication>> GetCuratorGroupApplications(int curatorId);
        
        Task DeleteStudentGroupApplicationById(int applicationId);

        Task DeleteStudentGroupApplication(StudentGroupApplication studentGroupApplication);
    }
}
