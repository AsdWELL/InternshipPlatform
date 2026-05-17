using InternshipPlatform.Application.Dtos.JobApplication;
using InternshipPlatform.Application.Dtos.Pagination;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Values;
using InternshipPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class JobApplicationRepository(InternshipPlatformContext context) : IJobApplicationRepository
    {
        public async Task<bool> HasStudentActiveApplicationOnVacancy(int resumeId, int vacancyId)
        {
            return await context.JobApplications
                .AsNoTracking()
                .AnyAsync(a => a.VacancyId == vacancyId
                            && a.Resume.StudentId == context.Resumes
                                .Where(r => r.Id == resumeId)
                                .Select(r => r.StudentId)
                                .FirstOrDefault()
                            && a.ApplicationStatusId != (int)JobApplicationStatuses.Rejected
                            && a.ApplicationStatusId != (int)JobApplicationStatuses.Withdrawn);
        }

        public async Task AddJobApplication(JobApplication application)
        {
            await context.JobApplications.AddAsync(application);
        }

        public async Task<JobApplication?> GetStudentApplicationForUpdate(int studentId, int applicationId)
        {
            return await context.JobApplications
                .FirstOrDefaultAsync(a =>
                    a.Id == applicationId &&
                    a.Resume.StudentId == studentId);
        }

        public async Task<JobApplication?> GetEmployerApplicationForUpdate(int employerId, int applicationId)
        {
            return await context.JobApplications
                .FirstOrDefaultAsync(a =>
                    a.Id == applicationId &&
                    a.Vacancy.CompanyId ==
                        context.EmployerProfiles
                            .Where(e => e.UserId == employerId)
                            .Select(e => e.CompanyId)
                            .FirstOrDefault());
        }

        public async Task<PagedResult<JobApplication>> GetStudentApplications(GetStudentApplicationsParameters request)
        {
            var query = context.JobApplications
                .AsNoTracking()
                .Where(a => a.Resume.StudentId == request.StudentId);

            if (request.ResumeId.HasValue)
                query = query.Where(a => a.ResumeId == request.ResumeId.Value);

            if (request.Status.HasValue)
                query = query.Where(a => a.ApplicationStatusId == (int)request.Status.Value);

            var totalCount = await query.CountAsync();

            var items = await query
                .Include(a => a.ApplicationStatus)
                .Include(a => a.Vacancy)
                    .ThenInclude(v => v.Company)
                .OrderByDescending(a => a.LastStatusDate)
                .Skip(request.PageIndex * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return new PagedResult<JobApplication>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        public async Task<PagedResult<JobApplication>> GetEmployerApplications(GetEmployerApplicationsParameters request)
        {
            var query = context.JobApplications
                .AsNoTracking()
                .Where(a => a.Vacancy.CompanyId ==
                        context.EmployerProfiles
                            .Where(e => e.UserId == request.EmployerId)
                            .Select(e => e.CompanyId)
                            .FirstOrDefault());

            if (request.VacancyId.HasValue)
                query = query.Where(a => a.VacancyId == request.VacancyId.Value);

            if (request.Status.HasValue)
                query = query.Where(a => a.ApplicationStatusId == (int)request.Status.Value);

            var totalCount = await query.CountAsync();

            var items = await query
                .Include(a => a.ApplicationStatus)
                .Include(a => a.Resume)
                    .ThenInclude(r => r.StudentProfile)
                .Include(a => a.Resume)
                    .ThenInclude(r => r.Specialization)
                .OrderByDescending(a => a.LastStatusDate)
                .Skip(request.PageIndex * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return new PagedResult<JobApplication>
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}
