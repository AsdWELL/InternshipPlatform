using InternshipPlatform.Application.Dtos.Pagination;
using InternshipPlatform.Application.Dtos.PracticeOffer;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Interfaces.Repositories
{
    public interface IPracticeOfferRepository
    {
        Task<bool> IsPracticeOfferExistsAndActive(int practiceOfferId);

        Task<bool> IsEmployerOwnsPracticeOffer(int employerId, int practiceOfferId);

        Task AddPracticeOffer(PracticeOffer practiceOffer);

        Task<List<PracticeOffer>> GetCompanyPracticeOffers(int companyId);

        Task<PracticeOffer?> GetPracticeOfferById(int practiceOfferId);

        Task<PracticeOffer?> GetPracticeOfferForUpdate(int practiceOfferId);

        Task<PagedResult<PracticeOffer>> SearchPracticeOffers(SearchPracticeOfferParameters parameters);

        Task DeletePracticeOffer(int practiceOfferId);
    }
}
