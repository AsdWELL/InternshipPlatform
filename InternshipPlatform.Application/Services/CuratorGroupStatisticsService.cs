using InternshipPlatform.Application.Dtos.JobApplication;
using InternshipPlatform.Application.Dtos.Resume;
using InternshipPlatform.Application.Dtos.StudentGroup;
using InternshipPlatform.Application.Dtos.StudentProfile;
using InternshipPlatform.Application.Exceptions.StudentProfile;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Mappers;

namespace InternshipPlatform.Application.Services
{
    public class CuratorGroupStatisticsService(
        ICuratorGroupStatisticsRepository curatorGroupStatisticsRepository,
        IResumeRepository resumeRepository)
        : ICuratorGroupStatisticsService
    {
        private async Task ThrowIfCuratorDoesNotHaveAccess(CuratorGroupStatisticsRequest request)
        {
            if (!await curatorGroupStatisticsRepository
                .IsCuratorHasAccess(request.CuratorId, request.GroupId, request.StudentId))
                throw new StudentProfileNotFoundException();
        }
        
        public async Task<CuratorStudentProfileDetails> GetStudentDetails(CuratorGroupStatisticsRequest request)
        {
            await ThrowIfCuratorDoesNotHaveAccess(request);

            var result = await curatorGroupStatisticsRepository.GetStudentDetails(request.StudentId);

            return result.ToDetails();
        }

        public async Task<List<ResumeCuratorItem>> GetStudentResumes(CuratorGroupStatisticsRequest request)
        {
            await ThrowIfCuratorDoesNotHaveAccess(request);

            var resumes = await resumeRepository.GetStudentResumes(request.StudentId);

            return resumes.Select(r => r.ToCuratorItem()).ToList();
        }

        public async Task<StudentApplicationsStatistics> GetApplicationsStatistics(CuratorGroupStatisticsRequest request)
        {
            await ThrowIfCuratorDoesNotHaveAccess(request);

            return await curatorGroupStatisticsRepository.GetStudentApplicationsStatistics(request.StudentId);
        }
    }
}
