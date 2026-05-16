using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.PracticeOffer
{
    public class PracticeOfferNotFoundException() : NotFoundException("Предложение практики не найдено");
}
