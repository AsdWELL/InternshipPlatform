using InternshipPlatform.Application.Dtos.JobApplication;
using InternshipPlatform.Application.Dtos.StudentProfile;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface ICuratorGroupStatisticsRepository
    {
        Task<bool> IsCuratorHasAccess(int curatorId, int groupId, int studentId);

        Task<StudentStatisticsResult> GetStudentDetails(int studentId);

        Task<StudentApplicationsStatistics> GetStudentApplicationsStatistics(int studentId);
    }
}
