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

        Task<List<PracticeOfferResult>> GetCompanyPracticeOffers(int companyId);

        Task<PracticeOfferResult?> GetPracticeOfferById(int practiceOfferId);

        Task<PracticeOfferResult?> GetPracticeOfferWithMaterialsById(int practiceOfferId);

        Task<PracticeOfferResult?> GetPracticeOfferForUpdate(int practiceOfferId);

        Task<PagedResult<PracticeOfferResult>> SearchPracticeOffers(SearchPracticeOfferParameters parameters);

        Task DeletePracticeOffer(int practiceOfferId);
    }
}
