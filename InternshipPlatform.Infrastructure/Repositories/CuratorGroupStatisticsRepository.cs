using InternshipPlatform.Application.Dtos.JobApplication;
using InternshipPlatform.Application.Dtos.StudentProfile;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Values;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class CuratorGroupStatisticsRepository(InternshipPlatformContext context) : ICuratorGroupStatisticsRepository
    {
        public Task<bool> IsCuratorHasAccess(int curatorId, int groupId, int studentId)
        {
            return context.StudentProfiles
                .AsNoTracking()
                .AnyAsync(sp =>
                    sp.UserId == studentId &&
                    sp.GroupId == groupId &&
                    sp.Group!.CuratorId == curatorId);
        }

        public Task<StudentStatisticsResult> GetStudentDetails(int studentId)
        {
            return context.StudentProfiles
                .AsNoTracking()
                .Include(sp => sp.User)
                .Where(sp => sp.UserId == studentId)
                .Select(sp => new StudentStatisticsResult
                {
                    StudentProfile = sp,
                    ResumesCount = context.Resumes
                        .Count(r => r.StudentId == sp.UserId),
                    ApplicationsCount = context.JobApplications
                        .Count(a => a.Resume.StudentId == sp.UserId)
                })
                .FirstAsync();
        }

        public async Task<StudentApplicationsStatistics> GetStudentApplicationsStatistics(int studentId)
        {
            var statusCounts = await context.JobApplications
                .AsNoTracking()
                .Where(a => a.Resume.StudentId == studentId)
                .GroupBy(a => a.ApplicationStatusId)
                .Select(g => new
                {
                    StatusId = g.Key,
                    Count = g.Count()
                })
                .ToDictionaryAsync(x => x.StatusId, x => x.Count);

            int GetCount(JobApplicationStatuses status)
            {
                return statusCounts.GetValueOrDefault((int)status);
            }

            return new StudentApplicationsStatistics
            {
                TotalApplications = statusCounts.Values.Sum(),
                InterviewInvitedCount = GetCount(JobApplicationStatuses.InterviewInvited),
                RejectedCount = GetCount(JobApplicationStatuses.Rejected),
                OfferReceivedCount = GetCount(JobApplicationStatuses.OfferReceived),
                EmployedCount = GetCount(JobApplicationStatuses.Employed)
            };
        }
    }
}
