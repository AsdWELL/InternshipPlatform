using InternshipPlatform.Application.Dtos.Pagination;
using InternshipPlatform.Application.Dtos.PracticeOffer;
using InternshipPlatform.Application.Exceptions.Company;
using InternshipPlatform.Application.Exceptions.PracticeOffer;
using InternshipPlatform.Application.Exceptions.Specialization;
using InternshipPlatform.Application.Interfaces;
using InternshipPlatform.Application.Interfaces.Repositories;
using InternshipPlatform.Application.Interfaces.Services;
using InternshipPlatform.Application.Mappers;
using InternshipPlatform.Application.Utils;

namespace InternshipPlatform.Application.Services
{
    public class PracticeOfferService(
        IPracticeOfferRepository practiceOfferRepository,
        ICompanyRepository companyRepository,
        ISpecializationRepository specializationRepository,
        IUnitOfWork unitOfWork) : IPracticeOfferService
    {
        private async Task<int> TryGetCompanyIdByEmployerId(int employerId)
        {
            var company = await companyRepository.GetCompanyByEmployerId(employerId)
                ?? throw new CompanyNotFoundException();

            return company.Id;
        }

        private async Task ThrowIfSpecializationNotExists(int id)
        {
            if (!await specializationRepository.IsSpecializationExists(id))
                throw new InvalidSpecializationException();
        }

        private async Task ThrowIfEmployerDoesNotOwnPracticeOffer(int employerId, int practiceOfferId)
        {
            if (!await practiceOfferRepository.IsEmployerOwnsPracticeOffer(employerId, practiceOfferId))
                throw new PracticeOfferNotFoundException();
        }

        public async Task<int> CreatePracticeOffer(CreatePracticeOfferRequest request)
        {
            await ThrowIfSpecializationNotExists(request.SpecializationId);

            var companyId = await TryGetCompanyIdByEmployerId(request.EmployerId);

            var practiceOffer = request.ToDomain(companyId);
            await practiceOfferRepository.AddPracticeOffer(practiceOffer);

            await unitOfWork.SaveChangesAsync();

            return practiceOffer.Id;
        }

        public async Task<List<PracticeOfferOwnerItem>> GetEmployerPracticeOffers(int employerId)
        {
            var companyId = await TryGetCompanyIdByEmployerId(employerId);

            var practiceOffers = await practiceOfferRepository.GetCompanyPracticeOffers(companyId);

            return practiceOffers.Select(po => po.ToOwnerItem()).ToList();
        }

        public async Task<List<PracticeOfferItem>> GetCompanyPracticeOffers(int companyId)
        {
            var practiceOffers = await practiceOfferRepository.GetCompanyPracticeOffers(companyId);

            return practiceOffers
                .Where(po => po.PracticeOffer.IsActive)
                .Select(po => po.ToItem())
                .ToList();
        }

        public async Task<PracticeOfferDetails> GetPracticeOfferDetailsForStudent(int practiceOfferId)
        {
            var result = await practiceOfferRepository.GetPracticeOfferById(practiceOfferId)
                ?? throw new PracticeOfferNotFoundException();

            if (!result.PracticeOffer.IsActive)
                throw new PracticeOfferNotFoundException();

            return result.ToDetails();
        }

        public async Task<PracticeOfferOwnerDetails> GetPracticeOfferDetailsForOwner(int employerId, int practiceOfferId)
        {
            await ThrowIfEmployerDoesNotOwnPracticeOffer(employerId, practiceOfferId);

            var practiceOffer = await practiceOfferRepository.GetPracticeOfferWithMaterialsById(practiceOfferId)
                ?? throw new PracticeOfferNotFoundException();

            return practiceOffer.ToOwnerDetails();
        }

        public async Task<PagedResponse<PracticeOfferItem>> SearchPracticeOffers(SearchPracticeOfferParameters parameters)
        {
            var result = await practiceOfferRepository.SearchPracticeOffers(parameters);

            return result.ToPagedResponse(parameters, po => po.ToItem());
        }

        public async Task UpdatePracticeOffer(UpdatePracticeOfferRequest request)
        {
            await ThrowIfEmployerDoesNotOwnPracticeOffer(request.EmployerId, request.Id);

            if (request.SpecializationId.HasValue)
                await ThrowIfSpecializationNotExists(request.SpecializationId.Value);

            var result = await practiceOfferRepository.GetPracticeOfferForUpdate(request.Id)
                ?? throw new PracticeOfferNotFoundException();

            var practiceOffer = result.PracticeOffer;

            if (request.MaxStudents is not null)
            {
                var applicationsCount = practiceOffer.MaxStudents - result.AvailablePlacesCount;

                if (request.MaxStudents < applicationsCount)
                    throw new InvalidPracticeOfferMaxStudentsCountException(applicationsCount, request.MaxStudents.Value);
                
                practiceOffer.MaxStudents = request.MaxStudents.Value;
            }

            if (!string.IsNullOrWhiteSpace(request.Title))
                practiceOffer.Title = StringNormalizer.NormalizeRequired(request.Title);

            if (request.Description is not null)
                practiceOffer.Description = StringNormalizer.NormalizeOptional(request.Description);

            if (request.IsRemote is not null)
                practiceOffer.IsRemote = request.IsRemote.Value;

            if (request.Region is not null)
                practiceOffer.Region = StringNormalizer.NormalizeOptional(request.Region);

            if (request.IsActive is not null)
                practiceOffer.IsActive = request.IsActive.Value;

            if (request.SpecializationId is not null)
                practiceOffer.SpecializationId = request.SpecializationId.Value;

            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeletePracticeOffer(int employerId, int practiceOfferId)
        {
            await ThrowIfEmployerDoesNotOwnPracticeOffer(employerId, practiceOfferId);

            await practiceOfferRepository.DeletePracticeOffer(practiceOfferId);

            await unitOfWork.SaveChangesAsync();
        }
    }
}
