using InternshipPlatform.Application.Dtos.Pagination;
using InternshipPlatform.Application.Dtos.PracticeOffer;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipPlatform.Infrastructure.Repositories
{
    public class PracticeOfferRepository(InternshipPlatformContext context) : IPracticeOfferRepository
    {
        public async Task<bool> IsPracticeOfferExistsAndActive(int practiceOfferId)
        {
            return await context.PracticeOffers
                .AsNoTracking()
                .AnyAsync(po => po.Id == practiceOfferId && po.IsActive);
        }

        public async Task<bool> IsEmployerOwnsPracticeOffer(int employerId, int practiceOfferId)
        {
            return await context.EmployerProfiles
                .AsNoTracking()
                .Where(ep => ep.UserId == employerId)
                .Join(
                    context.PracticeOffers.AsNoTracking(),
                    ep => ep.CompanyId,
                    po => po.CompanyId,
                    (ep, po) => po)
                .AnyAsync(po => po.Id == practiceOfferId);
        }

        public async Task AddPracticeOffer(PracticeOffer practiceOffer)
        {
            await context.PracticeOffers.AddAsync(practiceOffer);
        }

        public async Task<List<PracticeOfferResult>> GetCompanyPracticeOffers(int companyId)
        {
            return await context.PracticeOffers
                .AsNoTracking()
                .Where(po => po.CompanyId == companyId)
                .Include(po => po.Company)
                .Include(po => po.Specialization)
                .OrderByDescending(po => po.Id)
                .Select(o => new PracticeOfferResult
                {
                    PracticeOffer = o,
                    AvailablePlacesCount = o.MaxStudents -
                        context.StudentPractices.Count(sp => sp.PracticeOfferId == o.Id)
                })
                .ToListAsync();
        }

        public async Task<PracticeOfferResult?> GetPracticeOfferById(int practiceOfferId)
        {
            return await context.PracticeOffers
                .AsNoTracking()
                .Where(po => po.Id == practiceOfferId)
                .Include(po => po.Company)
                .Include(po => po.Specialization)
                .Select(o => new PracticeOfferResult
                {
                    PracticeOffer = o,
                    AvailablePlacesCount = o.MaxStudents -
                        context.StudentPractices.Count(sp => sp.PracticeOfferId == o.Id)
                })
                .FirstOrDefaultAsync();
        }

        public async Task<PracticeOfferResult?> GetPracticeOfferWithMaterialsById(int practiceOfferId)
        {
            return await context.PracticeOffers
                .AsNoTracking()
                .Where(po => po.Id == practiceOfferId)
                .Include(po => po.Company)
                .Include(po => po.Specialization)
                .Include(po => po.Materials)
                .Select(o => new PracticeOfferResult
                {
                    PracticeOffer = o,
                    AvailablePlacesCount = o.MaxStudents -
                        context.StudentPractices.Count(sp => sp.PracticeOfferId == o.Id)
                })
                .FirstOrDefaultAsync();
        }

        public async Task<PracticeOfferResult?> GetPracticeOfferForUpdate(int practiceOfferId)
        {
            return await context.PracticeOffers
                .Where(po => po.Id == practiceOfferId)
                .Select(o => new PracticeOfferResult
                {
                    PracticeOffer = o,
                    AvailablePlacesCount = o.MaxStudents -
                        context.StudentPractices.Count(sp => sp.PracticeOfferId == o.Id)
                })
                .FirstOrDefaultAsync();
        }

        public async Task<PagedResult<PracticeOfferResult>> SearchPracticeOffers(SearchPracticeOfferParameters parameters)
        {
            IQueryable<PracticeOffer> query = context.PracticeOffers
                .AsNoTracking()
                .Where(po => po.IsActive)
                .Include(po => po.Company)
                .Include(po => po.Specialization);

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = parameters.Search.ToLower().Trim();

                var searchInTitle = parameters.SearchInTitle;
                var searchInDescription = parameters.SearchInDescription;
                var searchInCompanyName = parameters.SearchInCompanyName;

                if (!searchInTitle && !searchInDescription && !searchInCompanyName)
                    searchInTitle = true;

                query = query.Where(po =>
                    (searchInTitle && po.Title.ToLower().Contains(search)) ||
                    (searchInDescription && po.Description != null && po.Description.ToLower().Contains(search)) ||
                    (searchInCompanyName && po.Company.Name.ToLower().Contains(search)));
            }

            if (parameters.IsRemote is not null)
                query = query.Where(po => po.IsRemote == parameters.IsRemote.Value);

            if (!string.IsNullOrWhiteSpace(parameters.Region))
                query = query.Where(po => po.Region != null && po.Region.ToLower().Contains(parameters.Region.ToLower().Trim()));

            if (parameters.SpecializationId is not null)
                query = query.Where(po => po.SpecializationId == parameters.SpecializationId.Value);

            if (parameters.HidePracticesWithoutPlaces)
                query = query.Where(po => context.StudentPractices.Count(sp => sp.PracticeOfferId == po.Id) < po.MaxStudents);


            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(po => po.IsRemote)
                .ThenByDescending(po => po.Id)
                .Skip(parameters.PageIndex * parameters.PageSize)
                .Take(parameters.PageSize)
                .Select(o => new PracticeOfferResult
                {
                    PracticeOffer = o,
                    AvailablePlacesCount = o.MaxStudents -
                        context.StudentPractices.Count(sp => sp.PracticeOfferId == o.Id)
                })
                .ToListAsync();

            return new PagedResult<PracticeOfferResult>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        public async Task DeletePracticeOffer(int practiceOfferId)
        {
            var practiceOffer = await context.PracticeOffers.FindAsync(practiceOfferId);

            if (practiceOffer is null)
                return;

            context.PracticeOffers.Remove(practiceOffer);
        }
    }
}
