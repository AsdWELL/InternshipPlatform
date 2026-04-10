using InternshipPlatform.Application.Dtos.Pagination;
using InternshipPlatform.Application.Dtos.Vacancy;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class VacancyRepository(InternshipPlatformContext context) : IVacancyRepository
    {
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
                .FirstOrDefaultAsync(v => v.Id == vacancyId);
        }

        public async Task<Vacancy?> GetVacancyForUpdate(int vacancyId)
        {
            return await context.Vacancies.FindAsync(vacancyId);
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