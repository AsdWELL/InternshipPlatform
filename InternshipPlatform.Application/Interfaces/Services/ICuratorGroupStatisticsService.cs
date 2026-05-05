using InternshipPlatform.Application.Dtos.JobApplication;
using InternshipPlatform.Application.Dtos.Resume;
using InternshipPlatform.Application.Dtos.StudentGroup;
using InternshipPlatform.Application.Dtos.StudentProfile;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface ICuratorGroupStatisticsService
    {
        Task<CuratorStudentProfileDetails> GetStudentDetails(CuratorGroupStatisticsRequest request);

        Task<List<ResumeCuratorItem>> GetStudentResumes(CuratorGroupStatisticsRequest request);

        Task<StudentApplicationsStatistics> GetApplicationsStatistics(CuratorGroupStatisticsRequest request);
    }
}
