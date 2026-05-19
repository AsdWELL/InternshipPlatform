using InternshipPlatform.Application.Dtos.Company;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Dtos.PracticeOffer
{
    public class PracticeOfferDetails
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public bool IsRemote { get; set; }

        public string? Region { get; set; }

        public int AvailablePlacesCount { get; set; }

        public int MaxStudents { get; set; }

        public Specialization Specialization { get; set; }

        public CompanyResponse Company { get; set; }
    }
}
