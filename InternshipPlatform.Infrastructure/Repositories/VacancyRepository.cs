using InternshipPlatform.Application.Dtos.Pagination;
using InternshipPlatform.Application.Dtos.Vacancy;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class VacancyRepository(InternshipPlatformContext context) : IVacancyRepository
    {
        public async Task<bool> IsVacancyExistsAndActive(int vacancyId)
        {
            return await context.Vacancies
                .AsNoTracking()
                .AnyAsync(v => v.Id == vacancyId && v.IsActive);
        }


        public async Task AddVacancy(Vacancy vacancy)
        {
            await context.Vacancies.AddAsync(vacancy);
        }

        public async Task DeleteVacancy(int vacancyId)
        {
            var vacancy = await context.Vacancies.FindAsync(vacancyId);

            if (vacancy is null)
                return;

            context.Vacancies.Remove(vacancy);
        }

        public async Task<List<Vacancy>> GetCompanyVacancies(int companyId)
        {
            return await context.Vacancies
                .AsNoTracking()
                .Where(v => v.CompanyId == companyId)
                .Include(v => v.Company)
                .Include(v => v.Specialization)
                .ToListAsync();
        }

        public async Task<Vacancy?> GetVacancyById(int vacancyId)
        {
            return await context.Vacancies
                .AsNoTracking()
                .Include(v => v.Company)
                .Include(v => v.Specialization)
                .Include(v => v.Skills)
                .FirstOrDefaultAsync(v => v.Id == vacancyId);
        }

        public async Task<Vacancy?> GetVacancyForUpdate(int vacancyId)
        {
            return await context.Vacancies
                .Include(v => v.Skills)
                .FirstOrDefaultAsync(v => v.Id == vacancyId);
        }

        public async Task<bool> IsEmployerOwnsVacancy(int employerId, int vacancyId)
        {
            return await context.EmployerProfiles
                .AsNoTracking()
                .Where(ep => ep.UserId == employerId)
                .Join(
                    context.Vacancies.AsNoTracking(),
                    ep => ep.CompanyId,
                    v => v.CompanyId,
                    (ep, v) => v)
                .AnyAsync(v => v.Id == vacancyId);
        }

        public async Task<PagedResult<Vacancy>> GetRecommendedVacanciesForResume(int resumeId, int pageIndex, int pageSize)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var resumeCriteria = await context.Resumes
                .AsNoTracking()
                .Where(r => r.Id == resumeId && r.IsActive)
                .Select(r => new
                {
                    r.Id,
                    r.SpecializationId,
                    r.Region,
                    r.DesiredSalary,
                    SkillIds = r.Skills.Select(s => s.Id).ToList(),
                    TotalMonths = r.WorkExperiences.Sum(we =>
                        (((we.EndDateWork ?? today).Year - we.StartDateWork.Year) * 12) +
                        ((we.EndDateWork ?? today).Month - we.StartDateWork.Month) -
                        (((we.EndDateWork ?? today).Day < we.StartDateWork.Day) ? 1 : 0))
                })
                .FirstOrDefaultAsync();

            if (resumeCriteria is null)
                return new PagedResult<Vacancy>
                {
                    Items = [],
                    TotalCount = 0
                };

            var totalYears = resumeCriteria.TotalMonths / 12;
            var hasRegion = !string.IsNullOrWhiteSpace(resumeCriteria.Region);
            var region = resumeCriteria.Region;
            var resumeSkillIds = resumeCriteria.SkillIds.Distinct().ToList();

            IQueryable<Vacancy> baseQuery = context.Vacancies
                .AsNoTracking()
                .Where(v => v.IsActive)
                .Where(v => v.SpecializationId == resumeCriteria.SpecializationId)
                .Where(v => v.MinWorkExperienceYears <= totalYears);

            if (resumeCriteria.DesiredSalary.HasValue)
                baseQuery = baseQuery.Where(v =>
                    (v.SalaryFrom != null && v.SalaryFrom <= resumeCriteria.DesiredSalary)
                    || (v.SalaryTo != null && v.SalaryTo >= resumeCriteria.DesiredSalary));

            var rankedQuery = baseQuery
                .Select(v => new
                {
                    VacancyId = v.Id,
                    SkillsMatchCount = resumeSkillIds.Count == 0
                        ? 0
                        : v.Skills.Count(s => resumeSkillIds.Contains(s.Id)),
                    RegionScore = hasRegion
                        ? (v.Region == region ? 3 :
                           v.IsRemote ? 2 : 1)
                        : (v.IsRemote ? 3 : 1),
                    v.Id
                })
                .Select(x => new
                {
                    x.VacancyId,
                    x.SkillsMatchCount,
                    x.RegionScore,
                    TotalScore = x.SkillsMatchCount * 10 + x.RegionScore,
                    x.Id
                });

            var totalCount = await rankedQuery.CountAsync();

            var vacancyIds = await rankedQuery
                .OrderByDescending(x => x.TotalScore)
                .ThenByDescending(x => x.SkillsMatchCount)
                .ThenByDescending(x => x.RegionScore)
                .ThenByDescending(x => x.Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(x => x.VacancyId)
                .ToListAsync();

            var vacancies = await context.Vacancies
                .AsNoTracking()
                .Where(v => vacancyIds.Contains(v.Id))
                .Include(v => v.Company)
                .Include(v => v.Specialization)
                .ToListAsync();

            var scoreMap = await rankedQuery
                .Where(x => vacancyIds.Contains(x.VacancyId))
                .ToDictionaryAsync(
                    x => x.VacancyId,
                    x => new
                    {
                        x.TotalScore,
                        x.SkillsMatchCount,
                        x.RegionScore,
                        x.Id
                    });

            var orderedVacancies = vacancies
                .OrderByDescending(v => scoreMap[v.Id].TotalScore)
                .ThenByDescending(v => scoreMap[v.Id].SkillsMatchCount)
                .ThenByDescending(v => scoreMap[v.Id].RegionScore)
                .ThenByDescending(v => scoreMap[v.Id].Id)
                .ToList();

            return new PagedResult<Vacancy>
            {
                Items = orderedVacancies,
                TotalCount = totalCount
            };
        }

        private async Task<PagedResult<Vacancy>> BuildFeedIfRecommendationsIsEmpty(int pageIndex, int pageSize)
        {
            var query = context.Vacancies
                .AsNoTracking()
                .Where(v => v.IsActive);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(v => v.IsRemote)
                .ThenByDescending(v => v.Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Include(v => v.Company)
                .Include(v => v.Specialization)
                .ToListAsync();

            return new PagedResult<Vacancy>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        public async Task<PagedResult<Vacancy>> GetRecommendedVacancies(int studentId, int pageIndex, int pageSize)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var resumes = await context.Resumes
                .AsNoTracking()
                .Where(r => r.StudentId == studentId && r.IsActive)
                .Select(r => new
                {
                    r.Id,
                    r.SpecializationId,
                    r.Region,
                    r.DesiredSalary,
                    SkillIds = r.Skills.Select(s => s.Id).ToList(),
                    TotalMonths = r.WorkExperiences.Sum(we =>
                        (((we.EndDateWork ?? today).Year - we.StartDateWork.Year) * 12) +
                        ((we.EndDateWork ?? today).Month - we.StartDateWork.Month) -
                        (((we.EndDateWork ?? today).Day < we.StartDateWork.Day) ? 1 : 0))
                })
                .ToListAsync();

            if (resumes.Count == 0)
                return await BuildFeedIfRecommendationsIsEmpty(pageIndex, pageSize);

            var specializationIds = resumes
                .Select(r => r.SpecializationId)
                .Distinct()
                .ToList();

            var candidateVacancies = await context.Vacancies
                .AsNoTracking()
                .Where(v => v.IsActive && specializationIds.Contains(v.SpecializationId))
                .Include(v => v.Company)
                .Include(v => v.Specialization)
                .Include(v => v.Skills)
                .ToListAsync();

            var rankedVacancies = candidateVacancies
                .Select(v =>
                {
                    var bestMatch = resumes
                        .Where(r =>
                            r.SpecializationId == v.SpecializationId &&
                            v.MinWorkExperienceYears <= (r.TotalMonths / 12) &&
                            (!r.DesiredSalary.HasValue ||
                             (v.SalaryFrom != null && v.SalaryFrom <= r.DesiredSalary.Value) ||
                             (v.SalaryTo != null && v.SalaryTo >= r.DesiredSalary.Value)))
                        .Select(r =>
                        {
                            var skillsMatchCount = r.SkillIds.Count == 0
                                ? 0
                                : v.Skills.Count(vs => r.SkillIds.Contains(vs.Id));

                            var regionScore = !string.IsNullOrWhiteSpace(r.Region)
                                ? (v.Region == r.Region ? 3 :
                                   v.IsRemote ? 2 : 1)
                                : (v.IsRemote ? 3 : 1);

                            var totalScore = skillsMatchCount * 10 + regionScore;

                            return new
                            {
                                Vacancy = v,
                                TotalScore = totalScore,
                                SkillsMatchCount = skillsMatchCount,
                                RegionScore = regionScore
                            };
                        })
                        .OrderByDescending(x => x.TotalScore)
                        .ThenByDescending(x => x.SkillsMatchCount)
                        .ThenByDescending(x => x.RegionScore)
                        .ThenByDescending(x => x.Vacancy.Id)
                        .FirstOrDefault();

                    return bestMatch;
                })
                .Where(x => x is not null)
                .Select(x => x!)
                .GroupBy(x => x.Vacancy.Id)
                .Select(g => g
                    .OrderByDescending(x => x.TotalScore)
                    .ThenByDescending(x => x.SkillsMatchCount)
                    .ThenByDescending(x => x.RegionScore)
                    .ThenByDescending(x => x.Vacancy.Id)
                    .First())
                .OrderByDescending(x => x.TotalScore)
                .ThenByDescending(x => x.SkillsMatchCount)
                .ThenByDescending(x => x.RegionScore)
                .ThenByDescending(x => x.Vacancy.Id)
                .ToList();

            if (rankedVacancies.Count == 0)
                return await BuildFeedIfRecommendationsIsEmpty(pageIndex, pageSize);

            var totalCount = rankedVacancies.Count;

            var items = rankedVacancies
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(x => x.Vacancy)
                .ToList();

            return new PagedResult<Vacancy>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        public async Task<PagedResult<Vacancy>> SearchVacancies(SearchVacancyParameters parameters)
        {
            IQueryable<Vacancy> query = context.Vacancies
                .AsNoTracking()
                .Where(v => v.IsActive)
                .Include(v => v.Company)
                .Include(v => v.Specialization);

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = parameters.Search.ToLower().Trim();

                var searchInTitle = parameters.SearchInTitle;
                var searchInDescription = parameters.SearchInDescription;
                var searchInCompanyName = parameters.SearchInCompanyName;

                if (!searchInTitle && !searchInDescription && !searchInCompanyName)
                    searchInTitle = true;

                query = query.Where(v =>
                    (searchInTitle && v.Title.ToLower().Contains(search)) ||
                    (searchInDescription && v.Description != null && v.Description.ToLower().Contains(search)) ||
                    (searchInCompanyName && v.Company.Name.ToLower().Contains(search)));
            }

            if (parameters.SalaryFrom is not null)
                query = query.Where(v =>
                    v.SalaryTo != null &&
                    v.SalaryTo >= parameters.SalaryFrom.Value);

            if (parameters.SalaryTo is not null)
                query = query.Where(v =>
                    v.SalaryFrom != null &&
                    v.SalaryFrom <= parameters.SalaryTo.Value);

            if (parameters.IsRemote is not null)
                query = query.Where(v => v.IsRemote == parameters.IsRemote.Value);

            if (!string.IsNullOrWhiteSpace(parameters.Region))
                query = query.Where(v => v.Region != null && v.Region.ToLower().Contains(parameters.Region.ToLower().Trim()));

            if (parameters.MinWorkExperienceYears is not null)
                query = query.Where(v =>
                    v.MinWorkExperienceYears >= parameters.MinWorkExperienceYears.Value);

            if (parameters.MaxWorkExperienceYears is not null)
                query = query.Where(v =>
                    v.MinWorkExperienceYears <= parameters.MaxWorkExperienceYears.Value);

            if (parameters.SpecializationId is not null)
                query = query.Where(v => v.SpecializationId == parameters.SpecializationId.Value);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip(parameters.PageIndex * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new PagedResult<Vacancy>
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}