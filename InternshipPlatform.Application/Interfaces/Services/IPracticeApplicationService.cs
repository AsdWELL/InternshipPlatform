using InternshipPlatform.Application.Dtos.PracticeApplication;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface IPracticeApplicationService
    {
        Task<int> CreatePracticeApplication(CreatePracticeApplicationRequest request);

        Task<StudentPracticeApplicationItem?> GetCurrentStudentPracticeApplication(int studentId);

        Task CancelPracticeApplication(int studentId, int applicationId);

        Task<List<EmployerPracticeApplicationItem>> GetEmployerPracticeApplications(int employerId);

        Task<EmployerPracticeApplicationDetails> GetEmployerPracticeApplicationDetails(int employerId, int applicationId);

        Task AcceptPracticeApplication(int employerId, int applicationId);

        Task RejectPracticeApplication(int employerId, int applicationId);
    }
}