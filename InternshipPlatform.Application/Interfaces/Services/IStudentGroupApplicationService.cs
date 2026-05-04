using InternshipPlatform.Application.Dtos.StudentGroupApplication;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface IStudentGroupApplicationService
    {
        Task<int> CreateStudentGroupApplication(CreateStudentGroupApplicationRequest request);

        Task<StudentGroupApplicationItem?> GetStudentGroupApplication(int studentId);

        Task DeleteStudentGroupApplication(int studentId, int applicationId);

        Task<List<StudentGroupApplicationCuratorItem>> GetCuratorGroupApplications(int curatorId);

        Task AcceptGroupApplication(int curatorId, int applicationId);

        Task RejectGroupApplication(int curatorId, int applicationId);
    }
}
