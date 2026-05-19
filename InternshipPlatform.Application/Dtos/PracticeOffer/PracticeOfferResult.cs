namespace InternshipPlatform.Application.Dtos.PracticeOffer
{
    public class PracticeOfferResult
    {
        public Domain.Entities.PracticeOffer PracticeOffer { get; set; }

        public int AvailablePlacesCount { get; set; }
    }
}
