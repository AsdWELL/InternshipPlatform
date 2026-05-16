using InternshipPlatform.Application.Dtos.PracticeOffer;
using InternshipPlatform.Application.Utils;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Mappers
{
    public static class PracticeOfferMapper
    {
        public static PracticeOffer ToDomain(
            this CreatePracticeOfferRequest request,
            int companyId)
        {
            return new PracticeOffer
            {
                CompanyId = companyId,
                Title = StringNormalizer.NormalizeRequired(request.Title),
                Description = StringNormalizer.NormalizeOptional(request.Description),
                IsRemote = request.IsRemote,
                IsActive = true,
                Region = StringNormalizer.NormalizeOptional(request.Region),
                MaxStudents = request.MaxStudents,
                SpecializationId = request.SpecializationId
            };
        }

        public static PracticeOfferItem ToItem(this PracticeOffer practiceOffer)
        {
            return new PracticeOfferItem
            {
                Id = practiceOffer.Id,
                Title = practiceOffer.Title,
                IsRemote = practiceOffer.IsRemote,
                Specialization = practiceOffer.Specialization.Name,
                MaxStudents = practiceOffer.MaxStudents,
                CompanyId = practiceOffer.CompanyId,
                CompanyName = practiceOffer.Company.Name,
                CompanyLogoPath = practiceOffer.Company.LogoPath,
                Region = practiceOffer.Region
            };
        }

        public static PracticeOfferOwnerItem ToOwnerItem(this PracticeOffer practiceOffer)
        {
            return new PracticeOfferOwnerItem
            {
                Id = practiceOffer.Id,
                Title = practiceOffer.Title,
                IsRemote = practiceOffer.IsRemote,
                Specialization = practiceOffer.Specialization.Name,
                MaxStudents = practiceOffer.MaxStudents,
                CompanyName = practiceOffer.Company.Name,
                CompanyLogoPath = practiceOffer.Company.LogoPath,
                Region = practiceOffer.Region,
                IsActive = practiceOffer.IsActive
            };
        }

        public static PracticeOfferDetails ToDetails(this PracticeOffer practiceOffer)
        {
            return new PracticeOfferDetails
            {
                Id = practiceOffer.Id,
                Title = practiceOffer.Title,
                Description = practiceOffer.Description,
                IsRemote = practiceOffer.IsRemote,
                Region = practiceOffer.Region,
                MaxStudents = practiceOffer.MaxStudents,
                Specialization = practiceOffer.Specialization,
                Company = practiceOffer.Company.ToResponse()
            };
        }

        public static PracticeOfferOwnerDetails ToOwnerDetails(this PracticeOffer practiceOffer)
        {
            return new PracticeOfferOwnerDetails
            {
                Id = practiceOffer.Id,
                Title = practiceOffer.Title,
                Description = practiceOffer.Description,
                IsRemote = practiceOffer.IsRemote,
                Region = practiceOffer.Region,
                IsActive = practiceOffer.IsActive,
                MaxStudents = practiceOffer.MaxStudents,
                Specialization = practiceOffer.Specialization,
                Company = practiceOffer.Company.ToResponse()
            };
        }
    }
}
