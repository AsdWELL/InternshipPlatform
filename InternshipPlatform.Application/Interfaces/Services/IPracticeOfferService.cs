using InternshipPlatform.Application.Dtos.Pagination;
using InternshipPlatform.Application.Dtos.PracticeOffer;

namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface IPracticeOfferService
    {
        Task<int> CreatePracticeOffer(CreatePracticeOfferRequest request);

        Task<List<PracticeOfferOwnerItem>> GetEmployerPracticeOffers(int employerId);

        Task<List<PracticeOfferItem>> GetCompanyPracticeOffers(int companyId);

        Task<PracticeOfferDetails> GetPracticeOfferDetailsForStudent(int practiceOfferId);

        Task<PracticeOfferOwnerDetails> GetPracticeOfferDetailsForOwner(int employerId, int practiceOfferId);

        Task<PagedResponse<PracticeOfferItem>> SearchPracticeOffers(SearchPracticeOfferParameters parameters);

        Task UpdatePracticeOffer(UpdatePracticeOfferRequest request);

        Task DeletePracticeOffer(int employerId, int practiceOfferId);
    }
}
