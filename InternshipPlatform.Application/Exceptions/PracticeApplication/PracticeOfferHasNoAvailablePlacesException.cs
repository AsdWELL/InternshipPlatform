using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.PracticeApplication
{
    public class PracticeOfferHasNoAvailablePlacesException()
        : ConflictException("PracticeOfferId", "На выбранную практику нет свободных мест");
}
