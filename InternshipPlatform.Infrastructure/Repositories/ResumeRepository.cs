using InternshipPlatform.Application.Dtos.Pagination;
using InternshipPlatform.Application.Dtos.Resume;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class ResumeRepository(InternshipPlatformContext context) : IResumeRepository
    {
        public async Task<bool> IsResumeExistsAndActive(int resumeId)
        {
            return await context.Resumes
                .AsNoTracking()
                .AnyAsync(r => r.Id == resumeId && r.IsActive);
        }

        public async Task<bool> IsWorkExperienceExists(int workExperienceId)
        {
            return await context.WorkExperiences
                .AsNoTracking()
                .AnyAsync(we => we.Id == workExperienceId);
        }

        public async Task<bool> IsWorkExperienceBelongsToResume(int resumeId, int workExperienceId)
        {
            return await context.WorkExperiences
                .AsNoTracking()
                .AnyAsync(we => we.Id == workExperienceId && we.ResumeId == resumeId);
        }

        public async Task AddResume(Resume resume)
        {
            await context.Resumes.AddAsync(resume);
        }

        public async Task AddWorkExperience(WorkExperience workExperience)
        {
            await context.WorkExperiences.AddAsync(workExperience);
        }

        public async Task CopyResume(int resumeId)
        {
            var sourceResume = await context.Resumes
                .Include(r => r.Skills)
                .Include(r => r.WorkExperiences)
                .FirstOrDefaultAsync(r => r.Id == resumeId);

            if (sourceResume is null)
                return;

            var copiedResume = new Resume
            {
                StudentId = sourceResume.StudentId,
                Description = sourceResume.Description,
                DesiredSalary = sourceResume.DesiredSalary,
                Region = sourceResume.Region,
                SpecializationId = sourceResume.SpecializationId,
                LastUpdateDate = DateOnly.FromDateTime(DateTime.UtcNow),
                IsActive = sourceResume.IsActive,
                Skills = sourceResume.Skills.ToList(),
                WorkExperiences = sourceResume.WorkExperiences
                    .Select(we => new WorkExperience
                    {
                        CompanyName = we.CompanyName,
                        Profession = we.Profession,
                        StartDateWork = we.StartDateWork,
                        EndDateWork = we.EndDateWork,
                        WorkDescription = we.WorkDescription
                    })
                    .ToList()
            };

            await context.Resumes.AddAsync(copiedResume);
        }

        public async Task DeleteResume(int resumeId)
        {
            var resume = await context.Resumes.FindAsync(resumeId);

            if (resume is null)
                return;

            context.Resumes.Remove(resume);
        }

        public Task<bool> IsStudentOwnsResume(int studentId, int resumeId)
        {
            return context.Resumes
                .AsNoTracking()
                .AnyAsync(r => r.Id == resumeId && r.StudentId == studentId);
        }

        public async Task<Resume?> GetResumeById(int id)
        {
            return await context.Resumes
                .AsNoTracking()
                .Include(r => r.Specialization)
                .Include(r => r.StudentProfile)
                    .ThenInclude(sp => sp.User)
                .Include(r => r.StudentProfile)
                    .ThenInclude(sp => sp.Group)
                        .ThenInclude(g => g.EducationalProgram)
                .Include(r => r.Skills)
                .Include(r => r.WorkExperiences)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Resume?> GetResumeForUpdate(int id)
        {
            return await context.Resumes
                .Include(r => r.Skills)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<ResumeResult>> GetStudentResumes(int studentId)
        {
            return await context.Resumes
                .AsNoTracking()
                .Where(r => r.StudentId == studentId)
                .Include(r => r.Specialization)
                .Include(r => r.WorkExperiences)
                .Select(r => new ResumeResult
                {
                    Resume = r,
                    ApplicationsCount = r.Applications.Count(),
                    ViewsCount = r.Views.Count()
                })
                .OrderByDescending(r => r.Resume.Id)
                .ToListAsync();
        }

        public async Task<PagedResult<Resume>> GetRecommendedResumesForVacancy(int vacancyId, int pageIndex, int pageSize)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var vacancyCriteria = await context.Vacancies
                .AsNoTracking()
                .Where(v => v.Id == vacancyId && v.IsActive)
                .Select(v => new
                {
                    v.Id,
                    v.SpecializationId,
                    v.Region,
                    v.IsRemote,
                    v.SalaryFrom,
                    v.SalaryTo,
                    v.MinWorkExperienceYears,
                    SkillIds = v.Skills.Select(s => s.Id).ToList()
                })
                .FirstOrDefaultAsync();

            if (vacancyCriteria is null)
            {
                return new PagedResult<Resume>
                {
                    Items = [],
                    TotalCount = 0
                };
            }

            var vacancySkillIds = vacancyCriteria.SkillIds.Distinct().ToList();

            IQueryable<Resume> baseQuery = context.Resumes
                .AsNoTracking()
                .Where(r => r.IsActive)
                .Where(r => r.SpecializationId == vacancyCriteria.SpecializationId)
                .Select(r => new Resume
                {
                    Id = r.Id,
                    LastUpdateDate = r.LastUpdateDate,
                    Description = r.Description,
                    IsActive = r.IsActive,
                    DesiredSalary = r.DesiredSalary,
                    Region = r.Region,
                    SpecializationId = r.SpecializationId,
                    StudentId = r.StudentId,
                    StudentProfile = r.StudentProfile,
                    Skills = r.Skills,
                    WorkExperiences = r.WorkExperiences
                });

            var rankedQuery = context.Resumes
                .AsNoTracking()
                .Where(r => r.IsActive)
                .Where(r => r.SpecializationId == vacancyCriteria.SpecializationId)
                .Select(r => new
                {
                    ResumeId = r.Id,
                    r.LastUpdateDate,
                    r.Region,
                    r.DesiredSalary,
                    TotalMonths = r.WorkExperiences.Sum(we =>
                        (((we.EndDateWork ?? today).Year - we.StartDateWork.Year) * 12) +
                        ((we.EndDateWork ?? today).Month - we.StartDateWork.Month) -
                        (((we.EndDateWork ?? today).Day < we.StartDateWork.Day) ? 1 : 0)),
                    SkillsMatchCount = vacancySkillIds.Count == 0
                        ? 0
                        : r.Skills.Count(s => vacancySkillIds.Contains(s.Id))
                })
                .Where(x => (x.TotalMonths / 12) >= vacancyCriteria.MinWorkExperienceYears)
                .Where(x =>
                    x.DesiredSalary == 0 ||
                    vacancyCriteria.SalaryTo == 0 ||
                    x.DesiredSalary <= vacancyCriteria.SalaryTo)
                .Select(x => new
                {
                    x.ResumeId,
                    x.LastUpdateDate,
                    x.SkillsMatchCount,
                    RegionScore = vacancyCriteria.IsRemote
                        ? 3
                        : (x.Region == vacancyCriteria.Region ? 3 : 1),
                    TotalScore = x.SkillsMatchCount * 10 +
                                 (vacancyCriteria.IsRemote
                                    ? 3
                                    : (x.Region == vacancyCriteria.Region ? 3 : 1))
                });

            var totalCount = await rankedQuery.CountAsync();

            var resumeIds = await rankedQuery
                .OrderByDescending(x => x.TotalScore)
                .ThenByDescending(x => x.SkillsMatchCount)
                .ThenByDescending(x => x.RegionScore)
                .ThenByDescending(x => x.LastUpdateDate)
                .ThenByDescending(x => x.ResumeId)
                .Skip(pageIndex* pageSize)
                .Take(pageSize)
                .Select(x => x.ResumeId)
                .ToListAsync();

            var resumes = await context.Resumes
                .AsNoTracking()
                .Where(r => resumeIds.Contains(r.Id))
                .Include(r => r.StudentProfile)
                .Include(r => r.Specialization)
                .Include(r => r.WorkExperiences)
                .ToListAsync();

            var scoreMap = await rankedQuery
                .Where(x => resumeIds.Contains(x.ResumeId))
                .ToDictionaryAsync(
                    x => x.ResumeId,
                    x => new
                    {
                        x.TotalScore,
                        x.SkillsMatchCount,
                        x.RegionScore,
                        x.LastUpdateDate,
                        x.ResumeId
                    });

            var orderedResumes = resumes
                .OrderByDescending(r => scoreMap[r.Id].TotalScore)
                .ThenByDescending(r => scoreMap[r.Id].SkillsMatchCount)
                .ThenByDescending(r => scoreMap[r.Id].RegionScore)
                .ThenByDescending(r => scoreMap[r.Id].LastUpdateDate)
                .ThenByDescending(r => scoreMap[r.Id].ResumeId)
                .ToList();

            return new PagedResult<Resume>
            {
                Items = orderedResumes,
                TotalCount = totalCount
            };
        }

        private async Task<PagedResult<Resume>> BuildFeedIfRecommendationsIsEmpty(int pageIndex, int pageSize)
        {
            var query = context.Resumes
                .AsNoTracking()
                .Where(v => v.IsActive);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(r => r.LastUpdateDate)
                .ThenByDescending(r => r.Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Include(r => r.StudentProfile)
                .Include(r => r.Specialization)
                .Include(r => r.WorkExperiences)
                .ToListAsync();

            return new PagedResult<Resume>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        public async Task<PagedResult<Resume>> GetRecommendedResumes(int companyId, int pageIndex, int pageSize)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var vacancies = await context.Vacancies
                .AsNoTracking()
                .Where(v => v.CompanyId == companyId && v.IsActive)
                .Select(v => new
                {
                    v.Id,
                    v.SpecializationId,
                    v.Region,
                    v.IsRemote,
                    v.SalaryFrom,
                    v.SalaryTo,
                    v.MinWorkExperienceYears,
                    SkillIds = v.Skills.Select(s => s.Id).ToList()
                })
                .ToListAsync();

            if (vacancies.Count == 0)
                return await BuildFeedIfRecommendationsIsEmpty(pageIndex, pageSize);

            var specializationIds = vacancies
                .Select(v => v.SpecializationId)
                .Distinct()
                .ToList();

            var candidateResumes = await context.Resumes
                .AsNoTracking()
                .Where(r => r.IsActive && specializationIds.Contains(r.SpecializationId))
                .Include(r => r.StudentProfile)
                .Include(r => r.Specialization)
                .Include(r => r.WorkExperiences)
                .ToListAsync();

            var rankedResumes = candidateResumes
                .Select(r =>
                {
                    var totalMonths = r.WorkExperiences.Sum(we =>
                        (((we.EndDateWork ?? today).Year - we.StartDateWork.Year) * 12) +
                        ((we.EndDateWork ?? today).Month - we.StartDateWork.Month) -
                        (((we.EndDateWork ?? today).Day < we.StartDateWork.Day) ? 1 : 0));

                    var bestMatch = vacancies
                        .Where(v =>
                            v.SpecializationId == r.SpecializationId &&
                            (totalMonths / 12) >= v.MinWorkExperienceYears &&
                            (r.DesiredSalary == 0 ||
                                v.SalaryTo == 0 ||
                                r.DesiredSalary <= v.SalaryTo))
                        .Select(v =>
                        {
                            var skillsMatchCount = v.SkillIds.Count == 0
                                ? 0
                                : r.Skills.Count(rs => v.SkillIds.Contains(rs.Id));

                            var regionScore = v.IsRemote
                                ? 3
                                : (!string.IsNullOrWhiteSpace(r.Region) && r.Region == v.Region ? 3 : 1);

                            var totalScore = skillsMatchCount * 10 + regionScore;

                            return new
                            {
                                Resume = r,
                                TotalScore = totalScore,
                                SkillsMatchCount = skillsMatchCount,
                                RegionScore = regionScore
                            };
                        })
                        .OrderByDescending(x => x.TotalScore)
                        .ThenByDescending(x => x.SkillsMatchCount)
                        .ThenByDescending(x => x.RegionScore)
                        .ThenByDescending(x => x.Resume.LastUpdateDate)
                        .ThenByDescending(x => x.Resume.Id)
                        .FirstOrDefault();

                    return bestMatch;
                })
                .Where(x => x is not null)
                .Select(x => x!)
                .GroupBy(x => x.Resume.Id)
                .Select(g => g
                    .OrderByDescending(x => x.TotalScore)
                    .ThenByDescending(x => x.SkillsMatchCount)
                    .ThenByDescending(x => x.RegionScore)
                    .ThenByDescending(x => x.Resume.LastUpdateDate)
                    .ThenByDescending(x => x.Resume.Id)
                    .First())
                .OrderByDescending(x => x.TotalScore)
                .ThenByDescending(x => x.SkillsMatchCount)
                .ThenByDescending(x => x.RegionScore)
                .ThenByDescending(x => x.Resume.LastUpdateDate)
                .ThenByDescending(x => x.Resume.Id)
                .ToList();

            if (rankedResumes.Count == 0)
                return await BuildFeedIfRecommendationsIsEmpty(pageIndex, pageSize);

            var totalCount = rankedResumes.Count;

            var items = rankedResumes
                .Skip(pageIndex* pageSize)
                .Take(pageSize)
                .Select(x => x.Resume)
                .ToList();

            return new PagedResult<Resume>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        public async Task<PagedResult<Resume>> SearchResumes(SearchResumeParameters parameters)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            IQueryable<Resume> query = context.Resumes
                .AsNoTracking()
                .Where(r => r.IsActive);

            if (parameters.SalaryFrom is not null)
                query = query.Where(r =>
                    r.DesiredSalary != 0 &&
                    r.DesiredSalary >= parameters.SalaryFrom.Value);

            if (parameters.SalaryTo is not null)
                query = query.Where(r =>
                    r.DesiredSalary != 0 &&
                    r.DesiredSalary <= parameters.SalaryTo.Value);

            if (!string.IsNullOrWhiteSpace(parameters.Region))
                query = query.Where(r => r.Region != null && r.Region.ToLower().Contains(parameters.Region.ToLower().Trim()));

            if (parameters.SpecializationId is not null)
                query = query.Where(r => r.SpecializationId == parameters.SpecializationId.Value);

            if (parameters.SkillIds is { Count: > 0 })
            {
                var skillIds = parameters.SkillIds.Distinct().ToList();

                query = query.Where(r => skillIds.All(id => r.Skills.Any(s => s.Id == id)));
            }

            if (parameters.AgeFrom is not null)
            {
                var maxBirthday = today.AddYears(-parameters.AgeFrom.Value);

                query = query.Where(r =>
                    r.StudentProfile.BirthdayDate != null &&
                    r.StudentProfile.BirthdayDate <= maxBirthday);
            }

            if (parameters.AgeTo is not null)
            {
                var minBirthday = today.AddYears(-(parameters.AgeTo.Value + 1)).AddDays(1);

                query = query.Where(r =>
                    r.StudentProfile.BirthdayDate != null &&
                    r.StudentProfile.BirthdayDate >= minBirthday);
            }

            if (parameters.ResumeUpdatedDateFilter is not null)
            {
                switch (parameters.ResumeUpdatedDateFilter.Value)
                {
                    case ResumeUpdatedDateFilter.Last24Hours:
                        query = query.Where(r => r.LastUpdateDate >= today.AddDays(-1));
                        break;

                    case ResumeUpdatedDateFilter.Last3Days:
                        query = query.Where(r => r.LastUpdateDate >= today.AddDays(-3));
                        break;

                    case ResumeUpdatedDateFilter.LastWeek:
                        query = query.Where(r => r.LastUpdateDate >= today.AddDays(-7));
                        break;

                    case ResumeUpdatedDateFilter.LastMonth:
                        query = query.Where(r => r.LastUpdateDate >= today.AddMonths(-1));
                        break;

                    case ResumeUpdatedDateFilter.LastYear:
                        query = query.Where(r => r.LastUpdateDate >= today.AddYears(-1));
                        break;

                    case ResumeUpdatedDateFilter.CustomInterval:
                        if (parameters.UpdatedFrom is not null)
                            query = query.Where(r => r.LastUpdateDate >= parameters.UpdatedFrom.Value);

                        if (parameters.UpdatedTo is not null)
                            query = query.Where(r => r.LastUpdateDate <= parameters.UpdatedTo.Value);

                        break;

                    case ResumeUpdatedDateFilter.AllTime:
                    default:
                        break;
                }
            }

            IQueryable<Resume> filteredQuery = query;

            if (parameters.MinWorkExperienceYears is not null ||
                parameters.MaxWorkExperienceYears is not null)
            {
                var queryWithExperience = filteredQuery
                    .Select(r => new
                    {
                        Resume = r,
                        TotalMonths = r.WorkExperiences.Sum(we =>
                            (((we.EndDateWork ?? today).Year - we.StartDateWork.Year) * 12) +
                            ((we.EndDateWork ?? today).Month - we.StartDateWork.Month) -
                            (((we.EndDateWork ?? today).Day < we.StartDateWork.Day) ? 1 : 0))
                    });

                if (parameters.MinWorkExperienceYears is not null)
                    queryWithExperience = queryWithExperience.Where(x => x.TotalMonths >= parameters.MinWorkExperienceYears * 12);
                if (parameters.MaxWorkExperienceYears is not null)
                    queryWithExperience = queryWithExperience.Where(x => x.TotalMonths <= parameters.MaxWorkExperienceYears * 12);

                filteredQuery = queryWithExperience.Select(x => x.Resume);
            }

            var totalCount = await filteredQuery.CountAsync();

            var pageResumeIds = await filteredQuery
                .OrderByDescending(r => r.LastUpdateDate)
                .ThenByDescending(r => r.Id)
                .Skip(parameters.PageIndex * parameters.PageSize)
                .Take(parameters.PageSize)
                .Select(r => r.Id)
                .ToListAsync();

            var resumes = await context.Resumes
                .AsNoTracking()
                .Where(r => pageResumeIds.Contains(r.Id))
                .Include(r => r.StudentProfile)
                .Include(r => r.Specialization)
                .Include(r => r.WorkExperiences)
                .OrderByDescending(r => r.LastUpdateDate)
                .ThenByDescending(r => r.Id)
                .AsSplitQuery()
                .ToListAsync();

            return new PagedResult<Resume>
            {
                Items = resumes,
                TotalCount = totalCount
            };
        }

        public async Task<WorkExperience?> GetWorkExperienceForUpdate(int id)
        {
            return await context.WorkExperiences.FindAsync(id);
        }

        public async Task DeleteWorkExperience(int workExperienceId)
        {
            var we = await context.WorkExperiences.FindAsync(workExperienceId);

            if (we is null)
                return;

            context.WorkExperiences.Remove(we);
        }
    }
}
